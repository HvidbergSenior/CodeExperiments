import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import {
  Box,
  Button,
  CircularProgress,
  Paper,
  Popover,
  Stack,
  Typography,
} from "@mui/material";
import { useCallback, useEffect, useRef, useState } from "react";
import { CustomerAccessTreeView } from "../../components/customer-access-treeview/customer-access-tree-view";
import { CustomerNode } from "../../components/customer-access-treeview/customer-node";
import { DebouncedStandardTextfield } from "../../components/filter/debounced-standard-textfield";
import { theme } from "../../theme/theme";
import { commonTranslations } from "../../translations/common";
import { filterTranslations } from "../../translations/filter";
import { customerAdminPageTranslations } from "../../translations/pages/customer-admin-page-translations";
import { hasDecendantsOrSelfAMatch } from "../../util/mapping";
import { deepCompareArrays } from "../../util/arrays/array-deep-compare";
import { customerPortalTranslations } from "../../translations/pages/customer-portal-translations";

interface Props {
  availableAccounts: CustomerNode[] | undefined;
  selectedCustomers: string[];
  updateFilter: (selectedCustomers: string[]) => void;
  updateSearchPredicate: (value: string) => void;
  searchPredicate: string;
  loadingAvailableAccounts: boolean;
}
export const AccountsTreeViewButton = ({
  availableAccounts,
  selectedCustomers,
  updateFilter,
  updateSearchPredicate,
  searchPredicate,
  loadingAvailableAccounts,
}: Props) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [predicate, setPredicate] = useState<string>(searchPredicate);
  const anchorRef = useRef<HTMLButtonElement>(null);
  const [filteredCustomers, setFilteredCustomers] = useState<
    CustomerNode[] | undefined
  >(availableAccounts);
  const [selectedNodes, setSelectedNodes] =
    useState<string[]>(selectedCustomers);
  const [selectedCustomerNames, setSelectedCustomerNames] = useState<number>(
    selectedCustomers.length,
  );

  useEffect(() => {
    setSelectedCustomerNames(selectedCustomers.length);
  }, [selectedCustomers]);

  useEffect(() => {
    setSelectedNodes(selectedCustomers);
  }, [selectedCustomers]);

  useEffect(() => {
    if (availableAccounts) {
      handleFilterCustomers(searchPredicate);
    }
  }, [searchPredicate, availableAccounts]);

  const handleCancel = useCallback(() => {
    setIsOpen(false);
    setSelectedNodes(selectedCustomers);
  }, [setSelectedNodes, selectedNodes, selectedCustomers]);

  const handleApply = useCallback(() => {
    const isSelectedNodesQual = deepCompareArrays(
      selectedNodes,
      selectedCustomers,
    );
    if (!isSelectedNodesQual) {
      updateSearchPredicate(predicate ?? "");
      updateFilter(selectedNodes);
    }

    setIsOpen(false);
  }, [updateFilter, selectedNodes, predicate, selectedCustomers]);

  const handleFilterCustomers = (filter: string) => {
    setPredicate(filter);
    filterCustomers(filter);
  };

  const filterCustomers = useCallback(
    (filter: string) => {
      let mutableBaseCustomers = availableAccounts
        ? [...availableAccounts]
        : undefined;
      if (mutableBaseCustomers === undefined) {
        mutableBaseCustomers = availableAccounts;
      }
      if (filter.trim() === "") {
        setFilteredCustomers(mutableBaseCustomers);
        return;
      }

      const filtered = mutableBaseCustomers?.filter((customer) =>
        hasDecendantsOrSelfAMatch(customer, filter),
      );
      setFilteredCustomers(filtered);
    },
    [availableAccounts, setPredicate, searchPredicate],
  );

  return (
    <div>
      <Button
        ref={anchorRef}
        onClick={() => setIsOpen(!isOpen)}
        sx={{
          borderRadius: "5px",
          border: isOpen ? "1px solid black" : `1px solid #c4c4c4`,
          width: { xs: "100%", sm: "240px" },
          height: { xs: "37px", sm: "37px" },
          minWidth: "130px",
          justifyContent: "space-between",
          "&:hover": {
            border: "1px solid black",
            backgroundColor: theme.palette.common.white,
          },
          "&:focus": {
            backgroundColor: theme.palette.common.white,
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
          {selectedCustomerNames > 0
            ? customerPortalTranslations.filter.accountsSelected(
                selectedCustomerNames,
              )
            : customerPortalTranslations.filter.accounts}
        </Typography>
        <ArrowDropDownIcon />
      </Button>
      <Popover
        open={isOpen}
        anchorEl={anchorRef.current}
        onClose={() => handleCancel()}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left",
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "left",
        }}
      >
        <Box
          sx={{
            width: { xs: "auto", sm: "450px" },
            height: { xs: "auto", sm: "500px" },
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
            m: theme.spacing(5),
          }}
        >
          <Paper
            style={{
              width: "100%",
              height: "420px",
              overflow: "auto",
              border: "1px solid #CCCCCC",
              background: "white",
              padding: 8,
            }}
          >
            <DebouncedStandardTextfield
              label={customerAdminPageTranslations.addUserDialog.customerSearch}
              startValue={predicate ? predicate : ""}
              onUpdate={(value) => handleFilterCustomers(value)}
            />

            <Box height="1rem" />
            {loadingAvailableAccounts ? (
              <Box
                display="flex"
                justifyContent="center"
                alignItems="center"
                width="100%"
                height="300px"
              >
                <CircularProgress />
              </Box>
            ) : (availableAccounts ?? []).length > 0 ? (
              <CustomerAccessTreeView
                data={filteredCustomers ?? []}
                selectedNodes={selectedNodes}
                setSelectedNodes={setSelectedNodes}
              />
            ) : (
              <Typography ml={2}>Failed to load available customers</Typography>
            )}
          </Paper>

          <Stack
            gap="16px"
            sx={{
              display: "flex",
              flexDirection: "row",
              justifyContent: "end",
              alignItems: "center",
            }}
          >
            <Button sx={{ width: "140px" }} onClick={handleCancel}>
              {commonTranslations.cancel}
            </Button>
            <Button sx={{ width: "140px" }} onClick={() => handleApply()}>
              {filterTranslations.apply}
            </Button>
          </Stack>
        </Box>
      </Popover>
    </div>
  );
};
