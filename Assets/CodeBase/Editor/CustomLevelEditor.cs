using CodeBase.Gameplay.Field.Config;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class CustomLevelEditor : UnityEditor.Editor
    {
        private VisualElement _blockPanel;
        private VisualElement _grid;
        
        private int _selectedBlock = -1;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("_id"), "ID"));

            var size = new PropertyField(serializedObject.FindProperty("_gridSize"), "Size");
            root.Add(size);
            size.RegisterValueChangeCallback(_ => RebuildGrid());
            var blocks = new PropertyField(serializedObject.FindProperty("_blocks"), "Blocks");
            root.Add(blocks);
            root.Add(new PropertyField(serializedObject.FindProperty("_grid"), "Grid"));
            blocks.RegisterValueChangeCallback(_ => BlockPanel());
            _grid = new VisualElement();
            RebuildGrid();
            
            _blockPanel = new VisualElement();
            
            _blockPanel.style.flexDirection = FlexDirection.Row;
            root.Add(_blockPanel);
            BlockPanel();
            
            root.Add(_grid);    
            return root;
        }

        private void BlockPanel()
        {
            _blockPanel.Clear();
            _blockPanel.Add(new Button(CleanGrid){text = "Clean"});
            _blockPanel.Add(new Button(() => OnSelectBlock(-1)){text = "None"});
            var blocks = serializedObject.FindProperty("_blocks");
            for (int i = 0; i < blocks.arraySize; i++)
            {
                var element = blocks.GetArrayElementAtIndex(i);
                var block = element.objectReferenceValue as BlockConfig;
                if (block != null)
                {
                    Button blockBtn = new Button(() =>
                    {
                        OnSelectBlock(block.ID);    
                    })
                    {
                        text = $"{block.ID} {block.Name}"
                    };
                    blockBtn.style.width = 40;
                    blockBtn.style.height = 40;
                    blockBtn.style.fontSize = 10;
                    _blockPanel.Add(blockBtn);
                }
            }
        }
        
        private void RebuildGrid()
        {
            _grid.Clear();
            _grid.style.flexDirection = FlexDirection.Column;
            _grid.style.alignItems = Align.Center;
            var title = new Label("Grid");
            title.style.fontSize = 20;
            title.style.paddingBottom = 10;
            _grid.Add(title);
            _grid.style.marginTop = 20;
            
            var size = serializedObject.FindProperty("_gridSize");
            var value = size.vector2IntValue;
            var grid = serializedObject.FindProperty("_grid");

            if (grid.arraySize != value.x * value.y)
            {
                grid.ClearArray();
                for (int i = 0; i < value.x * value.y; i++)
                {
                    grid.InsertArrayElementAtIndex(i);
                    grid.GetArrayElementAtIndex(i).intValue = -1;
                    serializedObject.ApplyModifiedProperties();
                }
            }
            
            for (int y = value.y - 1; y >= -1; y--)
            {
                VisualElement row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.alignItems = Align.Center;
                if (y < 0)
                    row.Add(GetLabel(" "));
                else
                    row.Add(GetLabel($"{y}"));
                
                _grid.Add(row);

                for (int x = 0; x < value.x; x++)
                {
                    int index = y * value.x + x;
                    
                    if (y < 0)
                    {
                        row.Add(GetLabel($"{x}"));
                        continue;
                    }
                    
                    
                    int cellValue = grid.GetArrayElementAtIndex(index).intValue;
                    
                    string message = $"Cell {x}-{y}";
                    var cell = new Button()
                    {
                        text = cellValue >= 0 ? $"{cellValue}" : "X"
                    };
                    cell.clicked += SetBlock;
                    cell.style.width = 40;
                    cell.style.height = 40;
                    row.Add(cell);
                    continue;

                    void SetBlock()
                    {
                        grid.GetArrayElementAtIndex(index).intValue = _selectedBlock;
                        serializedObject.ApplyModifiedProperties();
                        cell.text = $"{_selectedBlock}";
                    }
                }
            }
        }

        private void OnSelectBlock(int x)
        {
            _selectedBlock = x;
        }

        private void CleanGrid()
        {
            var grid = serializedObject.FindProperty("_grid");
            for (int i = 0; i < grid.arraySize; i++)
                grid.GetArrayElementAtIndex(i).intValue = -1;
            serializedObject.ApplyModifiedProperties();
            RebuildGrid();
        }
        
        private Label GetLabel(string text)
        {
            Label empty = new Label(text);
            empty.style.width = 40;
            empty.style.height = 36;
            empty.style.unityTextAlign = TextAnchor.MiddleCenter;
            empty.style.marginLeft = 4;
            empty.style.marginRight = 4;
            empty.style.paddingLeft = 6;
            empty.style.paddingRight = 6;
            return empty;
        }
    }
}