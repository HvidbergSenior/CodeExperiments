import IconButton from "@mui/material/IconButton";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";

import MoreVertIcon from "@mui/icons-material/MoreVert";
import MenuItem from "@mui/material/MenuItem";
import { Fragment, MouseEvent, useState } from "react";
import { OutgoingDeclarationResponse } from "../../../../../api/api";
import { publishedTabTranslations } from "../../../../../translations/pages/published-tab-translations";
import { useViewPublishedDeclarationDialog } from "../view-published-declaration-dialog.tsx/use-view-published-declaration-dialog";

interface Props {
  declaration: OutgoingDeclarationResponse;
}

export function PublishedDeclarationRowMenu({ declaration }: Props) {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const isOpen = Boolean(anchorEl);
  const openPublishedDeclarationDialog = useViewPublishedDeclarationDialog();

  const handleOpenVertMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
    event.stopPropagation();
  };
  const handleCloseVertMenu = () => {
    setAnchorEl(null);
  };

  const handleViewPublishedDeclarationClick = () => {
    openPublishedDeclarationDialog(declaration);
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
            onClick={handleViewPublishedDeclarationClick}
            sx={{ padding: (theme) => theme.spacing(3) }}
          >
            <ListItemText>
              {publishedTabTranslations.viewPublishedDeclarationContextMenuItem}
            </ListItemText>
          </MenuItem>
        </div>
      </Menu>
    </Fragment>
  );
}
