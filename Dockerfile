FROM node:24-alpine/node
WORKDIR /app

COPY package.json .
RUN npm i
COPY . .
RUN npm run build

EXPOSE 4770
CMD ["npm", "run", "preview"]