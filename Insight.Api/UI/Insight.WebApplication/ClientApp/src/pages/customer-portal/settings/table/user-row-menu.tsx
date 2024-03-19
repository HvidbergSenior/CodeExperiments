import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";

import MoreVertIcon from "@mui/icons-material/MoreVert";
import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import { AllUserResponse } from "../../../../api/api";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";
import { useAddUserCustomerPortalDialog } from "../add-user-dialog/use-add-user-dialog";

interface Props {
  user: AllUserResponse;
}

export function CustomerPortalUserRowMenu({ user }: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const open = Boolean(anchorEl);
  const openAddUserDialog = useAddUserCustomerPortalDialog();

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const editUser = () => {
    handleCloseVertMenu();
    openAddUserDialog(user);
  };

  return (
    <Fragment>
      <IconButton onClick={handleOpenVertMenu}>
        <MoreVertIcon fontSize="small" />
      </IconButton>
      <Menu anchorEl={anchorEl} open={open} onClose={handleCloseVertMenu}>
        <div>
          <MenuItem
            onClick={editUser}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {customerAdminPageTranslations.editUserMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
      </Menu>
    </Fragment>
  );
}
