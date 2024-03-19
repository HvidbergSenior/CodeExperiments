import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";

import MoreVertIcon from "@mui/icons-material/MoreVert";
import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import { reconciliationTranslations } from "../../../../../translations/pages/reconcilliation-translations";
import { useDeclarationDetailsDialog } from "../hooks/use-declaration-details-dialog";

interface Props {
  incomingDeclarationId: string;
  readOnlyView?: boolean;
}

export function DeclarationRowMenu({
  incomingDeclarationId: declarationId,
  readOnlyView,
}: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const open = Boolean(anchorEl);
  const openDeclarationDetailsDialog = useDeclarationDetailsDialog();

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const viewDeclarationDetails = () => {
    handleCloseVertMenu();
    openDeclarationDetailsDialog(declarationId, false, readOnlyView);
  };

  const editDeclarationDetails = () => {
    handleCloseVertMenu();
    openDeclarationDetailsDialog(declarationId, true, readOnlyView);
  };

  return (
    <Fragment>
      <IconButton onClick={handleOpenVertMenu}>
        <MoreVertIcon fontSize="small"></MoreVertIcon>
      </IconButton>
      <Menu anchorEl={anchorEl} open={open} onClose={handleCloseVertMenu}>
        <div>
          <MenuItem
            onClick={viewDeclarationDetails}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {reconciliationTranslations.viewDeclarationMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
        <Divider />
        {!readOnlyView && (
          <div>
            <MenuItem
              onClick={editDeclarationDetails}
              sx={{ padding: (theme) => theme.spacing(3) }}
            >
              <ListItemText>
                {reconciliationTranslations.editDeclarationMenuItem}
              </ListItemText>
            </MenuItem>
          </div>
        )}
      </Menu>
    </Fragment>
  );
}
