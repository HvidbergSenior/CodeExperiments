import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";
import Divider from "@mui/material/Divider";

import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { reconciliationTranslations } from "../../../../../translations/pages/reconcilliation-translations";
import { IncomingDeclarationResponse } from "../../../../../api/api";
import { useDeclarationDetailsDialog } from "../hooks/use-declaration-details-dialog";

interface Props {
  reconciledDeclaration: IncomingDeclarationResponse;
}

export function ReconciliationRowMenu({ reconciledDeclaration }: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const open = Boolean(anchorEl);
  const openDeclarationDetailsDialog = useDeclarationDetailsDialog();

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
    event.stopPropagation();
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const viewDeclarationDetails = () => {
    handleCloseVertMenu();
    openDeclarationDetailsDialog(reconciledDeclaration.id, false);
  };

  const editDeclarationDetails = () => {
    handleCloseVertMenu();
    openDeclarationDetailsDialog(reconciledDeclaration.id, true);
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
      </Menu>
    </Fragment>
  );
}
