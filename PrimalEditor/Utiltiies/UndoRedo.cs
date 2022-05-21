using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PrimalEditor.Utiltiies
{
    public interface IUndoRedo
    {
        string Name { get; }
        void Undo();
        void Redo();
    }

    public class UndoRedoAction : IUndoRedo
    {
        private Action _undoAction;
        private Action _redoAction;

        public string Name { get; }

        public void Redo() => _redoAction();

        public void Undo() => _undoAction();

        public UndoRedoAction(string name)
        {
            Name = name;
        }
        
        public UndoRedoAction(Action undo, Action redo, string name)
            : this(name)
        {
            Debug.Assert(undo != null && redo != null);
            _undoAction = undo;
            _redoAction = redo;
        }

        public UndoRedoAction(string property, object instance, object undoValue, object redoValue, string name) :
            this(
                () => instance.GetType().GetProperty(property).SetValue(instance, undoValue),
                () => instance.GetType().GetProperty(property).SetValue(instance, redoValue),
                name)
        { }
     
    }
    public class UndoRedo
    {
        private bool _enableAdd = true;
        private readonly ObservableCollection<IUndoRedo> _redoList = new ObservableCollection<IUndoRedo>();
        private readonly ObservableCollection<IUndoRedo> _undoList = new ObservableCollection<IUndoRedo>();
        public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; }
        public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; }

        public void Reset()
        {
            _redoList.Clear();
            _undoList.Clear();
        }

        public void Add(IUndoRedo cmd) // add new thing to list.
        {
            if (_enableAdd)
            {
                _undoList.Add(cmd);
                _redoList.Clear(); // clears redo list.
            }
        }

        public void Undo()
        { 
            if(_undoList.Any())
            {
                var cmd = _undoList.Last(); // get last command
                _undoList.RemoveAt(_undoList.Count - 1); // remove it from list
                _enableAdd = false;
                cmd.Undo(); // undo?
                _enableAdd = true;
                _redoList.Insert(0, cmd); // add to redolist
            }
        }
        
        public void Redo()
        {
            if (_redoList.Any())
            {
                var cmd = _redoList.First(); // get first redo
                _redoList.RemoveAt(0); // remove it from list
                _enableAdd = false;
                cmd.Redo(); // undo?
                _enableAdd = true;
                _undoList.Add(cmd); // add to undolist
            }
        }

        public UndoRedo()
        {
            RedoList = new ReadOnlyObservableCollection<IUndoRedo>(_redoList);
            UndoList = new ReadOnlyObservableCollection<IUndoRedo>(_undoList);
        }
    }
}
