FROM mcr.microsoft.com/mssql/server

ARG PROJECT_DIR=/tmp/globomantics
RUN mkdir -p $PROJECT_DIR
WORKDIR $PROJECT_DIR
COPY InitializeGlobomanticsDb.sql .
COPY wait-for-it.sh .
COPY entrypoint.sh .
COPY setup.sh .

CMD ["/bin/bash", "entrypoint.sh"]