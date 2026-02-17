FROM node:24-alpine
WORKDIR /app

ENV APP_PORT=4770
ENV APP_HOSTNAME=solarwatch-frontend.onrender.com
ENV APP_BACKEND_API=https://solarwatch-api-csharp.onrender.com

COPY package.json .
RUN npm i
COPY . .
RUN npm run build

EXPOSE $APP_PORT

CMD ["npm", "run", "preview"]