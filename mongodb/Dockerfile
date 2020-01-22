FROM mongo

ENV MONGO_INITDB_DATABASE=StocksDB

COPY setupdb.js /docker-entrypoint-initdb.d/

EXPOSE 27017