using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NSem3PT34.Classes.Util;

namespace NSem3PT34.Classes.Structure
{
    public class Composition : ISubject
    {
        private List<Glyph> children;
        private List<IObserver> observers;

        public Composition()
        {
            this.children = new List<Glyph>();
            this.observers = new List<IObserver>();
        }

        public void Insert(Glyph glyph, int i)
        {
            this.children.Insert(i, glyph);
        }

        public void Remove(int i)
        {
            this.children.RemoveAt(i);
        }


        public List<Glyph> GetChildren()
        {
            return this.children;
        }

        public void UpdateFont(List<Font> fonts, int startFrom, int endAt)
        {
            int i, j;
            for (i = startFrom, j = 0; i <= endAt; i++, j++)
            {
                this.children[i].SetFont(fonts[j]);
            }

            this.ModelChanged();
        }

        public void RegisterObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            int index = this.observers.IndexOf(observer);
            if (index >= 0)
            {
                this.observers.RemoveAt(index);
            }
        }

        public void NotifyObservers()
        {
            foreach (IObserver observer in this.observers)
            {
                observer.UpdateObserver();
            }
        }

        public void ModelChanged()
        {
            this.NotifyObservers();
        }

    }
}
