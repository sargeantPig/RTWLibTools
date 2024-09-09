namespace RTWLibPlus.parsers.objects;

using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using static RTWLibPlus.dataWrappers.BaseWrapper;
using static RTWLibPlus.parsers.DepthParse;
public static class Creator
{
    public static readonly ObjectCreator EDUcreator = (value, tag, depth) => new EDUObj(tag, value, depth);
    public static readonly ObjectCreator DRcreator = (value, tag, depth) => new DRObj(tag, value, depth);
    public static readonly ObjectCreator DScreator = (value, tag, depth) => new DSObj(tag, value, depth);
    public static readonly ObjectCreator EDBcreator = (value, tag, depth) => new EDBObj(tag, value, depth);
    public static readonly ObjectCreator SMFcreator = (value, tag, depth) => new SMFObj(tag, value, depth);
    public static readonly ObjectCreator DMBcreator = (value, tag, depth) => new DMBObj(tag, value, depth);
    public static readonly WrapperCreator DSWrapper = (data, config) => new DS(data, config);
    public static readonly WrapperCreator DRWrapper = (data, config) => new DR(data, config);
    public static readonly WrapperCreator DMBWrapper = (data, config) => new DMB(data, config);
    public static readonly WrapperCreator SMFWrapper = (data, config) => new SMF(data, config);
    public static readonly WrapperCreator EDBWrapper = (data, config) => new EDB(data, config);
    public static readonly WrapperCreator EDUWrapper = (data, config) => new EDU(data, config);
}

public static class Instance
{
    public static DS InstanceDS(TWConfig config, params string[] path) => (DS)RFH.CreateWrapper(Creator.DScreator, Creator.DSWrapper, config, ' ', false, path);
    public static DR InstanceDR(TWConfig config, params string[] path) => (DR)RFH.CreateWrapper(Creator.DRcreator, Creator.DRWrapper, config, '\t', false, path);
    public static DMB InstanceDMB(TWConfig config, params string[] path) => (DMB)RFH.CreateWrapper(Creator.DMBcreator, Creator.DMBWrapper, config, ' ', false, path);
    public static SMF InstanceSMF(TWConfig config, params string[] path) => (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, config, ':', false, path);
    public static EDB InstanceEDB(TWConfig config, params string[] path) => (EDB)RFH.CreateWrapper(Creator.EDBcreator, Creator.EDBWrapper, config, ' ', false, path);
    public static EDU InstanceEDU(TWConfig config, params string[] path) => (EDU)RFH.CreateWrapper(Creator.EDUcreator, Creator.EDUWrapper, config, ' ', false, path);

}
