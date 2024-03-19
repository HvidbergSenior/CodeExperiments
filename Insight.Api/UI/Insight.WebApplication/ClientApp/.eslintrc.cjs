module.exports = {
  root: true,
  env: { browser: true, es2020: true },
  extends: [
    'eslint:recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:react-hooks/recommended',
    "plugin:react/recommended",
    "plugin:react/jsx-runtime",
    "react-app",
    "react-app/jest",
    "prettier"
  ],
  ignorePatterns: ['dist', '.eslintrc.cjs'],
  parser: '@typescript-eslint/parser',
  plugins: ['react-refresh','@typescript-eslint'],
  rules: {
    'react-refresh/only-export-components': [
      'warn',
      { allowConstantExport: true },
    ],
    "react/prop-types": "off",
    "react/style-prop-object": [
      "error",
      {
        "allow": [
          "FormattedNumber",
          "FormattedDateParts",
          "FormattedRelativeTime"
        ]
      }
    ],
    "no-restricted-imports": [
      "error",
      {
        "patterns": ["@mui/*/*/*"]
      }
    ],
    // TODO: remove for Prod
    "no-debugger": "off",
  },
}
