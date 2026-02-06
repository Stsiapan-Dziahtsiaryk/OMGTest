using CodeBase.Gameplay.MVP;

namespace CodeBase.Gameplay.Field
{
    public class FieldPresenter : PresenterBase<FieldView>
    {
        private readonly FieldModel _model;
        
        public FieldPresenter(
            FieldView view,
            FieldModel model) : base(view)
        {
            _model = model;
        }
        
        
    }
}