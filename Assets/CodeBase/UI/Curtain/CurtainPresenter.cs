using CodeBase.UI.MVP;

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
    }
}