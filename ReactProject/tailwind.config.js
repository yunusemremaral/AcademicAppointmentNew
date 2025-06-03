/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "#121212",
        secondary: "#f4f4f5",
      },
      fontFamily: {
        roboto: ['"Roboto Condensed"', 'sans-serif'],
        inter: ['Inter', 'sans-serif'],
        poppins: ['Poppins', 'sans-serif'],
        condiment: ['Condiment', 'cursive'],
        supermercado: ['"Supermercado One"', 'cursive'],
      },
      backgroundImage: {
        'primary-gradient': 'linear-gradient(135deg, #1e3a8a 15%, #1d4ed8 65%, #3b82f6 90%)',
        'border-gradient': 'linear-gradient(to right, #3b82f6, transparent 95%)',
        'active-blue': 'linear-gradient(135deg, #1e3a8a 15%, #1d4ed8 65%, #3b82f6 90%)',
        'logo-background': 'linear-gradient(145deg, rgba(59, 130, 246, 0.15), rgba(37, 99, 235, 0.04))',
        'button-blue': 'linear-gradient(to bottom, #2563eb, #60a5fa)',
        'background-white': 'linear-gradient(120deg, rgba(255, 255, 255, 1), rgba(248, 250, 252, 0.98))',
      },
      screens: {
        'max-h-screen-740': {'raw': '(max-height: 740px)'},
      },
    },
  },
  plugins: [],
}
