import React from "react";
import { authorizedHttpClient } from "../../api";
import useHandleNetworkError from "../../util/errors/use-handle-network-error";
import { CustomerPortalFilter } from "../../pages/customer-portal/customer-portal-context";

export function downloadContentAsFile(
  content: any,
  filename: string,
  contentType: string,
) {
  if (!contentType) contentType = "application/octet-stream";
  var a = document.createElement("a");
  var blob = new Blob([content], { type: contentType });
  a.href = window.URL.createObjectURL(blob);
  a.download = filename;
  a.click();
}

interface Props {
  setLoading: (value: React.SetStateAction<boolean>) => void;
  fileName: string;
  filter: CustomerPortalFilter;
}
export const useExcelDownload = ({ fileName, setLoading, filter }: Props) => {
  const { showErrorDialog } = useHandleNetworkError();

  const handleDownload = async () => {
    setLoading(true);
    try {
      const response =
        await authorizedHttpClient.api.getFuelConsumptionTransactionsExcelFile(
          {
            fromDate: filter.fromDate,
            toDate: filter.toDate,
            customerIds: filter.accountsIds,
            productNames: filter.fuels,
          },
          {
            format: "blob",
          },
        );
      downloadContentAsFile(
        response.data,
        fileName,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      );
    } catch (error) {
      showErrorDialog(error);
    } finally {
      setLoading(false);
    }
  };

  return { handleDownload };
};
