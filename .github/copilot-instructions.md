# RTWLibTools Copilot Instructions

## Purpose
RTWLibTools is a .NET 8 codebase for Rome Total War randomisation utilities.
It is structured around:
- A CLI application that orchestrates user commands and workflows.
- A reusable core library that parses game data files and applies randomisation logic.
- A test project that validates wrappers, modifiers, randomiser behavior, and helpers.

## Project Hierarchy And Flow
Solution file:
- RTWLib_CLI/RTWLib_Solution.sln

Projects:
1. RTWLib_CLI (executable)
   - Target: net8.0
   - Depends on RTWLibPlus
   - Hosts command loop and user-facing randomiser operations.
2. RTWLibPlus (library)
   - Target: net8.0
   - Provides parsers, wrappers, map logic, data objects, helpers, and randomiser algorithms.
3. RTWLib_Tests (test suite)
   - Target: net8.0 + MSTest
   - References both RTWLibPlus and RTWLib_CLI.

Dependency direction:
- RTWLib_CLI -> RTWLibPlus
- RTWLib_Tests -> RTWLibPlus + RTWLib_CLI

## Code Entry Point
Main entry point:
- RTWLib_CLI/Program.cs
- Method: Program.Main(string[] args)

Startup sequence in Program.Main:
1. Sets working directory to executable base directory.
2. Loads config files from randomiser_config via CMDProcess.LoadConfigs().
3. If no configs are found, exits with code 1.
4. Displays title and prompts user to select a config.
5. Loads selected config into TWConfig.
6. Instantiates command modules (RandCMD, Help) and registers them.
7. Enters an infinite read-eval-print loop using CMDProcess.CMDScreener().

## Important Directories And Responsibilities

Top-level:
- RTWLib_CLI/
  - User interaction, command dispatch, template execution, and runtime orchestration.
- RTWLibPlus/
  - Core domain library: data wrappers, parsers, randomiser algorithms, map generation, utilities.
- RTWLib_Tests/
  - Automated tests for helpers, wrappers, parsers, randomisation routines, and map/modifier logic.
- randomiser_config/
  - Runtime configuration JSON files used by CLI config selection.
- randomiser_templates/
  - Template command scripts executed by the templates screen.
- mod_files/
  - Mod data/assets used as source or target inputs during randomisation workflows.

Inside RTWLib_CLI:
- Program.cs
  - Application entry point and command loop.
- cmd/cmdProcess.cs
  - Command screening/routing and reflection-based method invocation.
  - Loads configs and keeps global module register.
- cmd/moduleRegister.cs
  - Stores registered module instances for command invocation.
- cmd/modules/
  - Help: user help screens.
  - RandCMD: high-level randomiser commands (initial setup, randomisation actions, output).
  - Search: currently scaffolded for search operations.
- cmd/screens/templates.cs
  - Loads templates from randomiser_templates and executes command sequences.
- input/
  - CLI input handling and validation.
- draw/
  - Console formatting helpers (borders/progress).

Inside RTWLibPlus:
- data/
  - Configuration and domain data objects (for example TWConfig).
- dataWrappers/
  - File-specific wrappers for RTW data files (EDU, EDB, DS, DR, SMF, DMB, TGA).
- parsers/
  - Parsing framework and config/object parser components.
- randomiser/
  - Core randomisation logic (RandDS, RandEDU, RandWrap).
- modifiers/
  - Deterministic file/map modifications.
- map/
  - City/faction map logic and Voronoi-style operations.
- helpers/
  - Shared utility functions for IO, arrays, strings, formatting, vectors, and colors.
- interfaces/
  - Common interfaces used by wrappers/base objects.

Inside RTWLib_Tests:
- wrappers/
  - Wrapper-specific parsing and behavior tests.
- randomised/
  - Randomisation behavior tests.
- map/
  - Map generation and city map tests.
- modifiers/
  - Modifier behavior tests.
- helper/, cli/, dummy/
  - Helper method tests, CLI-level tests, and test doubles.
- resources/
  - Static input fixtures copied to test output.

## How The Code Comes Together

Runtime wiring:
1. Program.Main loads a selected TWConfig from randomiser_config.
2. RandCMD is created with TWConfig.
3. RandCMD constructs wrappers (EDU, EDB, DS, DR, SMF, DMB, TGA) with load/save paths from TWConfig.GetPath().
4. InitialSetup parses and prepares wrapper data, creates city map state, and prepares fallback content.
5. User commands are entered in CLI and routed by CMDProcess:
   - Built-in commands: help, templates, run, back.
   - Module commands: reflection dispatch to module methods.
6. Randomiser methods in RTWLibPlus.randomiser mutate in-memory wrapper data.
7. Output command writes modified wrapper contents to configured output paths.

Command dispatch details:
- CMDProcess.CMDScreener handles global shortcuts and template commands first.
- Remaining commands are parsed by CMDProcess.ReadCMD, which:
  - Resolves module type by name.
  - Finds method by command token.
  - Converts string args to expected method parameter types.
  - Invokes the target module method via reflection.

Template flow:
- Templates screen loads all files in randomiser_templates.
- Each line in a template file is treated as a CLI command.
- Running a template executes each command through the same CMDScreener pipeline.

## Working Guidelines For Contributors
- Keep CLI concerns in RTWLib_CLI and domain logic in RTWLibPlus.
- Add parsing/randomisation logic to RTWLibPlus first, then expose via a CLI module method.
- Preserve command names and signatures where possible to avoid breaking templates.
- Add tests in RTWLib_Tests that mirror any new wrapper/parser/randomiser behavior.
- Validate path/config assumptions through TWConfig rather than hardcoding file paths.