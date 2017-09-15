using Engine.Mongo.Template;
using System.Collections.Generic;

namespace GrainImplement.Crawler.Osm
{
    public class OsmTraceTamplate : MemoryCacheTemplate
    {
        //目录数据表
        protected string _tableName_OsmTrace = "OsmTrace";

        #region 数据初始化

        protected List<OsmTrace> _osmTraceCollection = new List<OsmTrace>();

        public override void Inilization(string connectString)
        {
            if (!_hasInilized)
            {
                _dataBaseName = "OsmTrace";
                base.Inilization(connectString);
                _push.BuildUniqueIndex<OsmTrace>(_tableName_OsmTrace);
            }
            ExportFromDataBase<OsmTrace>(_tableName_OsmTrace, Callback_Category);
        }

        public void Callback_Category<T>(List<T> result)
        {
            _osmTraceCollection = result as List<OsmTrace>;
        }

        #endregion

        #region 内置

        #endregion

        #region 增加

        #endregion


        #region 查询


        #endregion

        #region 删除

        #endregion


        #region 修改

        #endregion

    }
}
