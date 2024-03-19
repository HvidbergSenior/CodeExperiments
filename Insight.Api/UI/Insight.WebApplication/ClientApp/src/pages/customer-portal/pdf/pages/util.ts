import { ProductNameEnumeration } from "../../../../api/api";
import { palette } from "../../../../theme/biofuel/palette";
import { theme } from "../../../../theme/theme";

export const getGreenPalette = [
  theme.palette.primary.main,
  theme.palette.primary.light,
  palette?.DeepForestGreen.main,
  palette?.FreshLime.main,
  palette?.MintyGreen.main,
  palette?.OliveDrab.main,
  palette?.SpringGreen.main,
  palette?.TealBlue.main,
  palette?.TurquioseBlue.main,
  palette?.FernGreen.main,
  palette?.HerbalGreen.main,
  palette?.MeadowGreen.main,
  palette?.SoftSage.main,
  palette?.LemonLime.main,
  palette?.DeepOlive.main,
  palette?.BlueGreen.main,
  palette?.SkyBlue.main,
  palette?.CeruleanBlue.main,
  palette?.SteelBlue.main,
  palette?.SlateBlue.main,
];

export const getFuelColor: Map<ProductNameEnumeration, string> = new Map([
  ["B100", theme.palette.primary.light],
  ["Hvo100", theme.palette.primary.main],
  ["Diesel", "#000000"],
  ["Petrol", theme.palette.DeepForestGreen.main],
  ["HvoDiesel", theme.palette.DeepOlive.main],
  ["Adblue", theme.palette?.CeruleanBlue.main],
  ["HeatingOil", theme.palette.MintyGreen.main],
  ["Other", theme.palette.SpringGreen.main],
  ["Unknown", theme.palette.FreshLime.main],
]);

export const fuelTypesMapper: Record<string, ProductNameEnumeration> = {
  Unknown: "Unknown",
  HVO100: "Hvo100",
  "HVO Diesel": "HvoDiesel",
  AdBlue: "Adblue",
  B100: "B100",
  Diesel: "Diesel",
  Petrol: "Petrol",
  "Heating Oil": "HeatingOil",
  Other: "Other",
};
