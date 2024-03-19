import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";

import MoreVertIcon from "@mui/icons-material/MoreVert";
import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import { AllocationResponse } from "../../../../../api/api";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";
import { useViewAllocationDialog } from "../view-allocation-dialog/use-view-allocation-dialog";

interface Props {
  allocation: AllocationResponse;
}

export function AllocationRowMenu({ allocation }: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const isOpen = Boolean(anchorEl);

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
    event.stopPropagation();
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const openViewAllocationDialog = useViewAllocationDialog();
  const handleViewAllocationClick = () => {
    openViewAllocationDialog(allocation);
    setAnchorEl(null);
  };

  return (
    <Fragment>
      <IconButton onClick={handleOpenVertMenu}>
        <MoreVertIcon fontSize="small"></MoreVertIcon>
      </IconButton>
      <Menu anchorEl={anchorEl} open={isOpen} onClose={handleCloseVertMenu}>
        <div>
          <MenuItem
            onClick={handleViewAllocationClick}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {allocationTabTranslations.viewAllocationContextMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
      </Menu>
    </Fragment>
  );
}
