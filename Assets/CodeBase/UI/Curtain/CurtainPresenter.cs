using CodeBase.UI.MVP;
using DG.Tweening;

namespace CodeBase.UI.Curtain
{
    public class CurtainPresenter : PresenterBase<CurtainView>
    {
        public CurtainPresenter(CurtainView view) : base(view)
        {
        }

        protected override void OnDispose()
        {
        }

        protected override void HandleShow()
        {
            base.HandleShow();
            View
                .CanvasGroup
                .DOFade(1, 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnKill(Window.Close);
        }
    }
}