import { Button, ButtonGroup } from "@mui/material";
import { useMatch, useNavigate } from "react-router-dom";
import { translations } from "../../../translations";
import { OutgoingSubPages } from "../../types";

export const OutgoingButtonGroup = () => {
  const navigate = useNavigate();
  const match = useMatch("/outgoing/*");
  const selectedButton =
    (match?.params["*"] as OutgoingSubPages) || "outgoing-tab";

  const isButtonSelected = (id: string) => selectedButton === id;
  return (
    <ButtonGroup size="small" variant="text" aria-label="text button group">
      <Button
        sx={{
          fontWeight: isButtonSelected("outgoing-tab") ? 700 : "inherit",
        }}
        onClick={() => navigate("outgoing-tab")}
      >
        {
          translations.outgoingPageTranslations.outgoingTabTranslations
            .outgoingTabTitle
        }
      </Button>
      <Button
        sx={{
          fontWeight: isButtonSelected("stock") ? 700 : "inherit",
        }}
        onClick={() => navigate("stock")}
      >
        {
          translations.outgoingPageTranslations.allocationTabTranslations
            .stockTabTitle
        }
      </Button>
      <Button
        sx={{
          fontWeight: isButtonSelected("allocation") ? 700 : "inherit",
        }}
        onClick={() => navigate("allocation")}
      >
        {
          translations.outgoingPageTranslations.allocationTabTranslations
            .allocationTabTitle
        }
      </Button>
      <Button
        sx={{
          fontWeight: isButtonSelected("published") ? 700 : "inherit",
        }}
        onClick={() => navigate("published")}
      >
        {
          translations.outgoingPageTranslations.publishedTabTranslations
            .publishedTabTitle
        }
      </Button>
    </ButtonGroup>
  );
};
