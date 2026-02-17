FROM node:24-alpine
WORKDIR /app
ENV APP_PORT=4770

COPY package.json .
RUN npm i
COPY . .
RUN npm run build

EXPOSE $APP_PORT
CMD ["npm", "run", "preview"]