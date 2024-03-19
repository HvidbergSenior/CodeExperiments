import { Button, ButtonGroup } from "@mui/material";
import { useMatch, useNavigate } from "react-router-dom";
import { translations } from "../../../translations";
import { IncomingSubPages } from "../../types";

export const IncomingButtonGroup = () => {
  const navigate = useNavigate();
  const match = useMatch("/incoming/*");
  const selectedButton =
    (match?.params["*"] as IncomingSubPages) || "declaration_upload";

  const isButtonSelected = (id: string) => selectedButton === id;
  return (
    <ButtonGroup size="small" variant="text" aria-label="text button group">
      <Button
        sx={{
          fontWeight: isButtonSelected("declaration_upload") ? 700 : "inherit",
        }}
        onClick={() => navigate("declaration_upload")}
      >
        {
          translations.incomingTranslations.declarationUploadTranslations
            .declarationTabTitle
        }
      </Button>
      <Button
        sx={{
          fontWeight: isButtonSelected("approved") ? 700 : "inherit",
        }}
        onClick={() => navigate("approved")}
      >
        {
          translations.incomingTranslations.reconciliationTranslations
            .reconciliationTabTitle
        }
      </Button>
    </ButtonGroup>
  );
};
