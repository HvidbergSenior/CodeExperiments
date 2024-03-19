import { PaletteColor } from "@mui/material";
import { CSSProperties } from "react";

declare module "@mui/material/Button" {
  interface ButtonPropsVariantOverrides {
    dismiss: true;
  }
}

type CustomPaletteColor = Pick<PaletteColor, "main"> & Partial<PaletteColor>;

interface Colors {
  Green1: CustomPaletteColor;
  Green2: CustomPaletteColor;
  Green3: CustomPaletteColor;
  Green4: CustomPaletteColor;
  Green5: CustomPaletteColor;
  Green6: CustomPaletteColor;
  Green7: CustomPaletteColor;
  Green8: CustomPaletteColor;
  Green9: CustomPaletteColor;
  Green10: CustomPaletteColor;
  Green11: CustomPaletteColor;
  Green12: CustomPaletteColor;
  Green13: CustomPaletteColor;
  Green14: CustomPaletteColor;
  Green15: CustomPaletteColor;
  Green16: CustomPaletteColor;
  Gray1: CustomPaletteColor;
  Gray2: CustomPaletteColor;
  Gray3: CustomPaletteColor;
  Gray4: CustomPaletteColor;
  Yellow1: CustomPaletteColor;
  Yellow2: CustomPaletteColor;
  Purple1: CustomPaletteColor;
  Purple2: CustomPaletteColor;
  Red1: CustomPaletteColor;
  Red2: CustomPaletteColor;
  Red3: CustomPaletteColor;
  Blue1: CustomPaletteColor;
  Blue2: CustomPaletteColor;
  Background: CustomPaletteColor;
  Transparent: CustomPaletteColor;
  TransparentWhite: CustomPaletteColor;
  DeepForestGreen: CustomPaletteColor;
  FreshLime: CustomPaletteColor;
  MintyGreen: CustomPaletteColor;
  OliveDrab: CustomPaletteColor;
  SpringGreen: CustomPaletteColor;
  FernGreen: CustomPaletteColor;
  HerbalGreen: CustomPaletteColor;
  MeadowGreen: CustomPaletteColor;
  SoftSage: CustomPaletteColor;
  LemonLime: CustomPaletteColor;
  DeepOlive: CustomPaletteColor;
  BlueGreen: CustomPaletteColor;
  TealBlue: CustomPaletteColor;
  TurquioseBlue: CustomPaletteColor;
  SkyBlue: CustomPaletteColor;
  CeruleanBlue: CustomPaletteColor;
  SteelBlue: CustomPaletteColor;
  SlateBlue: CustomPaletteColor;
}

export type PaletteColors = Exclude<keyof Colors, "colors">;

declare module "@mui/material/styles/createPalette" {
  interface Palette extends Colors {}
  interface PaletteOptions extends Colors {}
}

interface TypographyExtension {
  subheading1: CSSProperties;
  datatable: CSSProperties;
  button1: CSSProperties;
  tooltip: CSSProperties;
  label: CSSProperties;
  error: CSSProperties;
  pdfHeaderTitle: CSSProperties;
  pdfPageTitle: CSSProperties;
  pdfSectionHeaderSmall: CSSProperties;
  pdfSectionBody: CSSProperties;
  pdfSectionBodyBold: CSSProperties;
  pdfSectionBodyLarge: CSSProperties;
  pdfSectionBodySmall: CSSProperties;
  pdfSectionBodySmaller: CSSProperties;
  pdfSectionBodySmallest: CSSProperties;
  body3: CSSProperties;
  body4: CSSProperties;
  pdf_h2: CSSProperties;
  pdf_h3: CSSProperties;
  pdf_h4: CSSProperties;
  pdf_h5: CSSProperties;
  pdf_h6: CSSProperties;
  pdf_body1: CSSProperties;
  pdf_body2: CSSProperties;
  pdf_body3: CSSProperties;
  pdf_body4: CSSProperties;
  pdf_subtitle1: CSSProperties;
}

declare module "@mui/material/styles" {
  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface TypographyVariants extends TypographyExtension {}

  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface TypographyVariantsOptions extends TypographyExtension {}
}

declare module "@mui/material/Typography" {
  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface TypographyPropsVariantOverrides
    extends Record<keyof TypographyExtension, true> {}
}
