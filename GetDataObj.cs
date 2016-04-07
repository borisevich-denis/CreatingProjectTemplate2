using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{
    class GetDataObj : IObserver<IDataObject>
    {
        private readonly IObjectsRepository _repository;
        private Guid gObj;
        public IDataObject Obj;

        public GetDataObj(IObjectsRepository repository, Guid _gObj)
        {
            gObj = _gObj;
            _repository = repository;
            _repository.SubscribeObjects(new[] { gObj }).Subscribe(this);
        }


        public IDataObject GetDataObject(Guid _gObj)
        {
            gObj = _gObj;
            _repository.SubscribeObjects(new[] { gObj }).Subscribe(this);
            return Obj;
        }

        public void OnNext(IDataObject value)
        {
            if (value.Id == gObj)
            {
                Obj = value;
            }
        }

        public void OnError(Exception error)
        {

        }

        public void OnCompleted()
        {

        }
    }
}
