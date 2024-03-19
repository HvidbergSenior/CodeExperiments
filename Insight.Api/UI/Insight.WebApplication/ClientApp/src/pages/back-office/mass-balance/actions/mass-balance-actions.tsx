import { Button, Menu, MenuItem, Stack, Typography } from "@mui/material";
import { ArrowDropDownIcon } from "@mui/x-date-pickers";
import { useState } from "react";
import {
  UnitOfMeasurement,
  useMassBalanceContext,
} from "../context/mass-balance-context";
import { capitalizeFirstLetter } from "../../../../util/formatters/formatters";

export function MassBalanceActions() {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const isUploadMenuOpen = Boolean(anchorEl);
  const { toggleUnitOfMeasurement, unitOfMeasurement } =
    useMassBalanceContext();

  const unitsOfMeasurement: UnitOfMeasurement[] = ["liter", "mt"];

  const handleClose = () => {
    setAnchorEl(null);
  };
  const handleUploadMenuItemClick = (unit: UnitOfMeasurement) => {
    toggleUnitOfMeasurement(unit);
    setAnchorEl(null);
  };

  const handleUploadClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  return (
    <Stack direction="row" spacing="10px">
      <Button
        onClick={handleUploadClick}
        sx={{
          borderRadius: "5px",
          border: anchorEl ? "1px solid black" : `1px solid #c4c4c4`,
          width: { xs: "100%", sm: "80px" },
          height: { xs: "37px", sm: "auto" },
          minWidth: "130px",
          justifyContent: "space-between",
          "&:hover": {
            border: "1px solid black",
            backgroundColor: (theme) => theme.palette.common.white,
          },
          "&:focus": {
            backgroundColor: (theme) => theme.palette.common.white,
            color: "black",
          },
        }}
      >
        <Typography
          sx={{ fontSize: "14px" }}
          color="grey"
          fontWeight={500}
          fontSize={16}
        >
          {capitalizeFirstLetter(unitOfMeasurement)}
        </Typography>
        <ArrowDropDownIcon />
      </Button>
      <Menu anchorEl={anchorEl} open={isUploadMenuOpen} onClose={handleClose}>
        {unitsOfMeasurement.map((unit, _) => (
          <MenuItem key={unit} onClick={() => handleUploadMenuItemClick(unit)}>
            {capitalizeFirstLetter(unit)}
          </MenuItem>
        ))}
      </Menu>
    </Stack>
  );
}
