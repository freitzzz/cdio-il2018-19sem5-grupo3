# Port where the server will be launched
SERVER_PORT=4000

# Runs MYCL HTTP Server
swipl -f http_server.pl -g "carregar,server($SERVER_PORT)"
