@echo off
if not exist target mkdir target
if not exist target\classes mkdir target\classes


echo compile classes
javac -nowarn -d target\classes -sourcepath jvm -cp "G:\Projects\jsCompiler\jsCompiler\jni4net\lib\compiler.jar";"g:\projects\jscompiler\jscompiler\jni4net\jni4net.j-0.8.6.0.jar"; "java\com/google/javascript/jscomp\JSError_.java" "java\com/google/javascript/jscomp\Result_.java" "java\com/google/javascript/jscomp\SourceFile_.java" "java\com/google/javascript/jscomp\JSSourceFile_.java" "java\com/google/javascript/jscomp\Compiler_.java" "java\com/google/javascript/jscomp\CompilerOptions_.java" "java\com/google/javascript/jscomp\CompilationLevel_.java" "java\com/google/javascript/jscomp\WarningLevel_.java" "java\com/google/javascript/jscomp\CheckLevel_.java" "java\com/google/javascript/jscomp\DiagnosticGroups_.java" "java\com/google/javascript/jscomp\CommandLineRunner_.java" 
IF %ERRORLEVEL% NEQ 0 goto end


echo proxygen.j4n.jar 
jar cvf proxygen.j4n.jar  -C target\classes "com\google\javascript\jscomp\JSError_.class"  -C target\classes "com\google\javascript\jscomp\Result_.class"  -C target\classes "com\google\javascript\jscomp\SourceFile_.class"  -C target\classes "com\google\javascript\jscomp\JSSourceFile_.class"  -C target\classes "com\google\javascript\jscomp\Compiler_.class"  -C target\classes "com\google\javascript\jscomp\CompilerOptions_.class"  -C target\classes "com\google\javascript\jscomp\CompilationLevel_.class"  -C target\classes "com\google\javascript\jscomp\WarningLevel_.class"  -C target\classes "com\google\javascript\jscomp\CheckLevel_.class"  -C target\classes "com\google\javascript\jscomp\DiagnosticGroups_.class"  -C target\classes "com\google\javascript\jscomp\CommandLineRunner_.class"  > nul 
IF %ERRORLEVEL% NEQ 0 goto end


echo proxygen.j4n.dll 
csc /nologo /warn:0 /t:library /out:proxygen.j4n.dll /recurse:clr\*.cs  /reference:"G:\Projects\jsCompiler\jsCompiler\jni4net\jni4net.n-0.8.6.0.dll"
IF %ERRORLEVEL% NEQ 0 goto end


:end
