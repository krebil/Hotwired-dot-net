/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["pages/**/*.cshtml","src/app.js","safelist.txt"],
  theme: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/line-clamp'),
      require('@tailwindcss/typography')
  ],
}
