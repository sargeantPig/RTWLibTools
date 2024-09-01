namespace RTWLib_CLI.cmd.modules;

using RTWLib_CLI;
using RTWLib_CLI.draw;
using RTWLibPlus.helpers;

public class Help
{
    string title;

    public string help()
    {
        string methods = CLIHelper.GetMethodList(typeof(Help));
        title = string.Format("RTWLib CLI\nHelp\n---\nTry the following commands in\nformat [name] [arg1] [arg2] etc\n---\n{0}", methods);

        return CLIHelper.ScreenChangeRTN(title);


    }

    public string Templates() => "You can view available templates by typing 'templates'\nYou can run a tempalte by typing 'run template_name.txt' where template_name is the name of the template you want to run";

    public string Randomiser() =>
    "The Randomiser tool can be navigated by typing commands, excluding the apostrophes. the following are some of the commands and descriptors.\n\n" +
    "'templates': Access the template menu to start a randomisation.\n" +
    "'help': The help menu, you're here right now.\n" +
    "'back': Go back a menu";
    public string Test(params string[] args)
    {
        string argsStr = args.ToString(',');
        return "response: " + argsStr;
    }

}
