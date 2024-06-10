﻿namespace RTWLibPlus.parsers.configs.whiteSpace;

public class WSConfigFactory
{
    public WhiteSpaceConfig CreateEDBWhiteSpace() => new(' ', 4);

    public WhiteSpaceConfig Create_EDU_DMB_WhiteSpace() => new(' ', 1);

    public WhiteSpaceConfig Create_DR_DS_SMF_WhiteSpace() => new('\t', 1);
}
