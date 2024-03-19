# BIOFUEL INSIGHT

## Quick Start

If you, the reader, discovers that the guide no longer is correct, you must ensure that the README is updated

~~_Ensure that the text in this section is simple enough that novices can understand it - prevents them from interrupting you more than is required_~~

### Prerequisites

- Run "npm install" in ClientApp root
- Install C# kit when you open vs-code for the first time.
- Install .net 7
- Add links where applicable
- Specify versions if needed
- Access Requirements (AD Groups, role assignments etc.)

Reminder : .net 7

### Setup Docker

In order to get connected to the backend, you need to run the docker container. Run then docker container by navigating to Insight.Api and enter ".\localDependenciesStart.ps1". Then docker will set up the database (if you are not allowed to run scripts in powershell then run "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser" in powershell (https://stackoverflow.com/questions/4647429/powershell-on-windows-7-set-executionpolicy-for-regular-users))

### Setup Mock Network

In the browser's dev tools, go to Application -> Local Storage and set a new key/ value pair : Key: MOCK_NETWORK value: true

## Code Formatting

In the react project, prettier is used to format code. The prettier package is part of the dev-dependencies and will be installed when you execute 'npm install'.
In order to get prettier to work, you may need to follow the step called "Formatting using VSCode on save" in this guide:
https://khalilstemmler.com/blogs/tooling/prettier/

The prettier settings should be part of the repo, but here they are:
{
"tabWidth": 2,
"semi": true,
"singleQuote": false,
"printWidth": 80
}

## Folder structure and organization

-- Types

Prop types are placed in the component file in which they are used. In order to avoid component files with a lot of type declarations, types other than props must be placed in the types.ts file in the root of the page folder or component folder depending on what makes sense. If a type is used in both pages and component folder, then place it in types.ts in the shared folder.

As much as possible, use types to create a strong contract to the types received from the backend.

### How to Run/Debug

You can run the projects in two ways - with or without backend.

- With backend: Open entire Insight project, navigate to program.cs in root, tap the play button.
- Without backend: In root folder, execute "npm start".

### Generate api code

In root folder, execute "npm run swagger".

## Troubleshooting

- Description of known errors (if any) and how to resolve them

## Documentation

- Link to a Confluence site or similar for the project where information can be found about:
  - Domain Description
  - Definition of Done (DoD)
  - Definition of Ready (DoR)
  - Test strategy and test processes.
- Link to Issue Tracker
- Branching and Merging Strategy?
- Version Control Conventional Commits?

### Architecture

- Drawings (Ideally rendered via PUML files) of the architecture in the C1 and C2 levels
- Description of the used principles (DDD, Event Driven, Vertical Slicing, Event Sourcing, Task-Based UI, etc.) - can just be a [link to the ADR](ADR)

### Architecture Decision Records (ADR)

- Link to ADR folder - should be in git

### Styleguide / Code Style

- Link to styleguide or code style file

## DevOps Setup

### Description of the Setup

### How to Deploy

### Monitoring

#### How to Access Logs

#### Metrics

#### Alarms

## Communication Channels

- Link to Teams channel / Slack / etc.
- Mailing Lists

## Mjølner's Development Model

- Link to [Way of Working](https://mjolner.atlassian.net/wiki/spaces/MWOW/overview)
- Link to [Development Model](https://mjolner.atlassian.net/wiki/spaces/MU/overview)

Based on template: [Link to template in Confluence]

# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## Expanding the ESLint configuration

If you are developing a production application, we recommend updating the configuration to enable type aware lint rules:

- Configure the top-level `parserOptions` property like this:

```js
   parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    project: ['./tsconfig.json', './tsconfig.node.json'],
    tsconfigRootDir: __dirname,
   },
```

- Replace `plugin:@typescript-eslint/recommended` to `plugin:@typescript-eslint/recommended-type-checked` or `plugin:@typescript-eslint/strict-type-checked`
- Optionally add `plugin:@typescript-eslint/stylistic-type-checked`
- Install [eslint-plugin-react](https://github.com/jsx-eslint/eslint-plugin-react) and add `plugin:react/recommended` & `plugin:react/jsx-runtime` to the `extends` list
