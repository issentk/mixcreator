using MixCreator.Event;

namespace MixCreator.Command
{
    internal class EventSelectionChanged : IEvent
    {
        public bool DirectionDown { get; set; }

        public EventSelectionChanged(bool directionDown)
        {
            this.DirectionDown = directionDown;
        }
    }
}