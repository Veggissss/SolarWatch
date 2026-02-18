FROM node:24-alpine
WORKDIR /app

ENV APP_PORT=4770
ENV APP_HOSTNAME=localhost
ENV APP_BACKEND_API=http://host.docker.internal:8080

COPY package.json .
RUN npm i
COPY . .
RUN npm run build

EXPOSE $APP_PORT

CMD ["npm", "run", "preview"]