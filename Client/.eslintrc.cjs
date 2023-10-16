module.exports = {
  env: {
    browser: true,
    es2022: true,
    "jest/globals": true,
    node: true,
  },
  extends: [
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:react/jsx-runtime",
    "react-app",
    "react-app/jest",
    "prettier",
  ],
  plugins: ["react", "react-hooks"],
  rules: {
    "react/display-name": [
      "off",
      {
        ignoreTranspilerName: true,
      },
    ],
    eqeqeq: "error",
    "react/prop-types": "off",
  },
  parser: "@babel/eslint-parser",
  parserOptions: {
    requireConfigFile: false,
    babelOptions: {
      presets: ["@babel/preset-react"],
    },
  },
  settings: {
    react: {
      version: "detect",
    },
  },
}
