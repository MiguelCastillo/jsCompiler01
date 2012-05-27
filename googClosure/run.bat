copy "lib\jni4net\jni4net.*" "./"
proxygen.exe proxygen.xml
call build.cmd
move "clr\com" "./"
move proxygen.j4n* lib/proxygen.j4n
del jni4net.*
del proxygen.proxygen.xml
del build.cmd
rmdir /S /Q target
rmdir /S /Q clr
rmdir /S /Q jvm