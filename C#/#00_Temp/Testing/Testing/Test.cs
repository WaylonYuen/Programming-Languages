using System;
using System.Collections.Generic;

namespace Testing {

    public struct Message {

        public string Text { get => m_text; }

        private string m_text;

        public Message(string newText) {
            m_text = newText;
        }

    }

    public class Headquarters : IObservable<Message> {

        private List<IObserver<Message>> observers;


        public Headquarters() {
            observers = new List<IObserver<Message>>();
        }


        public IDisposable Subscribe(IObserver<Message> observer) {

            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable {

            private List<IObserver<Message>> _observers;
            private IObserver<Message> _observer;

            public Unsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer) {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose() {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }

        }


        public void SendMessage(Nullable<Message> loc) {

            foreach (var observer in observers) {
                if (!loc.HasValue)
                    observer.OnError(new MessageUnknownException());
                else
                    observer.OnNext(loc.Value);
            }
        }

        public void EndTransmission() {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }

    public class MessageUnknownException : Exception {
        internal MessageUnknownException() {

        }
    }

    /// <summary>
    /// 檢查
    /// </summary>
    public class Inspector : IObserver<Message> {

        public string Name { get => instName; }

        private string instName;
        private IDisposable unsubscriber;

        public Inspector(string name) {
            instName = name;
        }

        public virtual void Subscribe(IObservable<Message> provider) {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted() {
            Console.WriteLine("The headquarters has completed transmitting data to {0}.", this.Name);
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e) {
            Console.WriteLine("{0}: Cannot get message from headquarters.", this.Name);
        }

        public virtual void OnNext(Message value) {
            Console.WriteLine("{1}: Message I got from headquarters: {0}", value.Text, this.Name);
        }

        public virtual void Unsubscribe() {
            unsubscriber.Dispose();
        }
    }

}
