import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";

import MoreVertIcon from "@mui/icons-material/MoreVert";
import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { useManualAllocationDialog } from "../manual-allocation-dialog/use-manual-allocation-dialog";
import { OutgoingFuelTransactionResponse } from "../../../../../api/api";

interface Props {
  fuelTransaction: OutgoingFuelTransactionResponse;
}

export function OutgoingRowMenu({ fuelTransaction }: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const isOpen = Boolean(anchorEl);

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
    event.stopPropagation();
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const openManualAllocationDialog = useManualAllocationDialog();
  const handleManualAllocationMenuItemClick = () => {
    openManualAllocationDialog(fuelTransaction);
    setAnchorEl(null);
  };

  const handleViewCustomerMenuItemClicked = () => {
    handleCloseVertMenu();
  };

  return (
    <Fragment>
      <IconButton onClick={handleOpenVertMenu}>
        <MoreVertIcon fontSize="small"></MoreVertIcon>
      </IconButton>
      <Menu anchorEl={anchorEl} open={isOpen} onClose={handleCloseVertMenu}>
        <div>
          <MenuItem
            onClick={handleManualAllocationMenuItemClick}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {outgoingTabTranslations.manualAllocationContextMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
        <Divider />
        <div>
          <MenuItem
            onClick={handleViewCustomerMenuItemClicked}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {outgoingTabTranslations.viewCustomerContextMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
      </Menu>
    </Fragment>
  );
}
