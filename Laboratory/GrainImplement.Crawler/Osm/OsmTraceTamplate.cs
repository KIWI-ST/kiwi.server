using Engine.Mongo.Template;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainImplement.Crawler.Osm
{
    public class OsmTraceTamplate : MemoryCacheTemplate
    {
        //目录数据表
        protected string _tableName_OsmTrace = "OsmTrace";

        int _count = 0;
        #region 数据初始化

        protected List<OsmTrace> _osmTraceCollection = new List<OsmTrace>();

        public override void Inilization(string connectString)
        {
            if (!_hasInilized)
            {
                _dataBaseName = "OSM";
                base.Inilization(connectString);
                //_push.BuildUniqueIndex<OsmTrace>(_tableName_OsmTrace);
            }
            ExportFromDataBase<OsmTrace>(_tableName_OsmTrace, Callback_Category);
        }

        public void Callback_Category<T>(List<T> result)
        {
            _osmTraceCollection = result as List<OsmTrace>;
        }

        #endregion

        #region 内置
        /// <summary>
        /// 显示当前总数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        #endregion

        #region 增加

        public async Task<bool> Enqueue(string content)
        {
            OsmTrace osmTrace = JsonConvert.DeserializeObject<OsmTrace>(content);
            osmTrace.Gpx = "www.openstreetmap.org/trace/" + osmTrace.TraceId + "/data";
            bool hasExist = _osmTraceCollection.Find(p => p.TraceId.Equals(osmTrace.TraceId)) != null;
            if (!hasExist)
            {
                var task = await _push.PushData<OsmTrace>(_tableName_OsmTrace, osmTrace);
                _osmTraceCollection.Add(osmTrace);
                _count++;
                return true;
            }
            return false;
        }

        #endregion

        #region 查询


        #endregion

        #region 删除

        #endregion

        #region 修改

        public async Task<bool> ModifyOsmTraceInfo(string targetId,string content)
        {
            var osmTrace = _osmTraceCollection.Find(p => p.TraceId.Equals(targetId));
            osmTrace.Gpx = content;
            var task = await _push.PushData<OsmTrace>(_tableName_OsmTrace, osmTrace);
            return task;
        }

        #endregion

    }
}
