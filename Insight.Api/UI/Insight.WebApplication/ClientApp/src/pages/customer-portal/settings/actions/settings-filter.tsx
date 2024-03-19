import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import { ClearIcon } from "@mui/x-date-pickers";
import { DebouncedTextfield } from "../../../../components/filter/debounced-textfield";
import { filterTranslations } from "../../../../translations/filter";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { useCustomerPortalContext } from "../../customer-portal-context";
import { UserStatus } from "../../../../api/api";

const AvailableUserStatus: { value: UserStatus; label: string }[] = [
  { value: "Active", label: "Active" },
  { value: "Blocked", label: "Blocked" },
  { value: "BlockedAndActive", label: "Blocked and Active" },
];

export const SettingsFilter = () => {
  const {
    setSettingsFilter,
    isSettingsFilterApplied,
    clearSettingsFilter,
    settingsFilter,
  } = useCustomerPortalContext();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: { xs: "column", sm: "row" },
        alignItems: "center",
        gap: "16px",
      }}
    >
      <DebouncedTextfield
        label={customerPortalTranslations.settings.filterEmail}
        startValue={settingsFilter.email}
        updateFilter={(value) =>
          setSettingsFilter((prev) => ({ ...prev, email: value }))
        }
      />
      <DebouncedTextfield
        label={customerPortalTranslations.settings.filterAccountId}
        startValue={settingsFilter.accountId}
        updateFilter={(value) =>
          setSettingsFilter((prev) => ({ ...prev, accountId: value }))
        }
      />
      <FormControl sx={{ width: { xs: "100%", sm: "200px" } }}>
        <InputLabel>
          {customerPortalTranslations.settings.filterStatus}
        </InputLabel>
        <Select
          label="fuels"
          value={settingsFilter.status}
          onChange={(value) =>
            setSettingsFilter((prev) => ({
              ...prev,
              status: value.target.value as UserStatus,
            }))
          }
        >
          {AvailableUserStatus.map((status, index) => {
            return (
              <MenuItem key={index} value={status.value}>
                {status.label}
              </MenuItem>
            );
          })}
        </Select>
      </FormControl>
      {isSettingsFilterApplied() && (
        <Button
          sx={{
            width: "140px",
            textWrap: "noWrap",
            backgroundColor: "transparent",
          }}
          variant="text"
          onClick={() => clearSettingsFilter()}
          startIcon={<ClearIcon fontSize="large" />}
        >
          {filterTranslations.clearFilterButtonTitle}
        </Button>
      )}
    </Box>
  );
};
