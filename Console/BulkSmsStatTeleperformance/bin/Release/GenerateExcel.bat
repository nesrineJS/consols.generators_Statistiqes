ECHO Start of Loop

FOR /L %%i IN (1,1,42) DO (
  ECHO %%i
  ping -n 10 127.0.0.1
)

