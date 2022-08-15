namespace Signals
{
    public class OnElementClickSignal
    {
        public readonly Element Element; //ссылка, по какому элементу кликнули(записать из конструктора, прочитать без возможности изменить)
        public OnElementClickSignal(Element element)
        {
            Element = element;
        }
    }
}