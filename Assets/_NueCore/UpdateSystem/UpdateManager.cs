using System.Collections.Generic;
using _NueCore.ManagerSystem.Core;

namespace _NueCore.UpdateSystem
{
    public class UpdateManager : NManagerBase
    {
        #region Fields

        private static List<IUpdateable> _updateTable = new List<IUpdateable>();
        private static List<IFixedUpdateable> _fixedUpdateTable = new List<IFixedUpdateable>();
        private static List<ILateUpdateable> _lateUpdateTable = new List<ILateUpdateable>();
        
        private static List<IUpdateable> _updateCleanTable = new List<IUpdateable>();
        private static List<IFixedUpdateable> _fixedUpdateCleanTable = new List<IFixedUpdateable>();
        private static List<ILateUpdateable> _lateUpdateCleanTable = new List<ILateUpdateable>();

        private static List<IUpdateable> _updateReadyTable = new List<IUpdateable>();
        private static List<IFixedUpdateable> _fixedUpdateReadyTable = new List<IFixedUpdateable>();
        private static List<ILateUpdateable> _lateUpdateReadyTable = new List<ILateUpdateable>();
        
        #endregion
        
        #region Methods

        public static void Register(IUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _updateReadyTable.Add(obj);
        }

        public static void Unregister(IUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _updateReadyTable.Remove(obj);
        }
        
        public static void Register(IFixedUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _fixedUpdateReadyTable.Add(obj);
        }

        public static void Unregister(IFixedUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _fixedUpdateReadyTable.Remove(obj);
        }
        
        public static void Register(ILateUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _lateUpdateReadyTable.Add(obj);
        }
        
        public static void Unregister(ILateUpdateable obj)
        {
            if(obj == null) 
                throw new System.ArgumentNullException();

            _lateUpdateReadyTable.Remove(obj);
        }

        void Update()
        {
            _updateCleanTable.Clear();
            _updateCleanTable.AddRange(_updateReadyTable);
            _updateTable.Clear();
            foreach (var updateable in _updateCleanTable)
            {
                if (updateable == null)
                    continue;
                _updateTable.Add(updateable);
            }

            foreach (var updateable in _updateTable)
            {
                if (updateable == null)
                    continue;
                updateable.ManagedUpdate();
            }
        }
        
        void FixedUpdate()
        {
            _fixedUpdateCleanTable.Clear();
            _fixedUpdateCleanTable.AddRange(_fixedUpdateReadyTable);
            _fixedUpdateTable.Clear();
            foreach (var updateable in _fixedUpdateCleanTable)
            {
                if (updateable == null)
                    continue;
                _fixedUpdateTable.Add(updateable);
            }

            foreach (var updateable in _fixedUpdateTable)
            {
                if (updateable == null)
                    continue;
                updateable.ManagedFixedUpdate();
            }
        }
        
        void LateUpdate()
        {
            _lateUpdateCleanTable.Clear();
            _lateUpdateCleanTable.AddRange(_lateUpdateReadyTable);
            _lateUpdateTable.Clear();
            foreach (var updateable in _lateUpdateCleanTable)
            {
                if (updateable == null)
                    continue;
                _lateUpdateTable.Add(updateable);
            }

            foreach (var updateable in _lateUpdateTable)
            {
                if (updateable == null)
                    continue;
                updateable.ManagedLateUpdate();
            }
        }

        #endregion

    }
}