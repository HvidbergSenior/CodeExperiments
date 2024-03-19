import React, { useState } from "react";
import { authorizedHttpClient } from "../../../../../api";
import { IncomingDeclarationSupplier } from "../../../../../api/api";
import { UploadState } from "../../../../../shared/types";

export type UploadResult = {
  batchId: string;
  fileName: string;
  errors: string[];
  state: UploadState;
  oldestEntryDate?: string;
  newestEntryDate?: string;
};

export const useUpload = (
  incomingDeclarationSupplier: IncomingDeclarationSupplier,
) => {
  const [isLoading, setIsLoading] = useState(false);
  const [uploadId, setUploadId] = useState<string | undefined>(undefined);
  const [declarationUploadResult, setDeclarationUploadResult] = useState<
    UploadResult | undefined
  >(undefined);

  const hiddenFileInputRef = React.createRef<HTMLInputElement>();

  const handleUploadClick = async () => {
    // Activate the hidden input field that triggers the file selector
    hiddenFileInputRef.current?.click();
  };

  const cancelUpload = async () => {
    if (!!uploadId) {
      try {
        await authorizedHttpClient.api.cancelIncomingDeclarationByUploadId(
          uploadId,
          { incomingDeclarationUploadId: uploadId },
        );
      } catch {}
    }
  };

  const handleFileChange = async (
    event: React.ChangeEvent<HTMLInputElement>,
  ) => {
    const files = event.target.files;

    if (files && files.length > 0) {
      const fileSelected = files[0];

      setIsLoading(true);

      const formData = new FormData();
      formData.append("file", fileSelected);

      try {
        const response =
          await authorizedHttpClient.api.incomingdeclarationsUploadCreate({
            ExcelFile: fileSelected,
            IncomingDeclarationSupplier: incomingDeclarationSupplier,
          });

        let errorList: string[] = [];

        response.data.incomingDeclarationParseResponses.forEach(
          (parseResponse) => {
            if (!parseResponse.success) {
              const errorMessage = `${parseResponse.errorMessage}, Row number: ${parseResponse.rowNumber}, PoS: ${parseResponse.posNumber}`;
              errorList.push(errorMessage);
            }
          },
        );

        const uploadResult: UploadResult = {
          batchId: response.data.incomingDeclarationUploadId.id ?? "-",
          errors: errorList,
          fileName: fileSelected.name,
          state: errorList.length > 0 ? "Error" : "Approved",
          oldestEntryDate: response.data.oldestEntry,
          newestEntryDate: response.data.newestEntry,
        };

        setUploadId(response.data.incomingDeclarationUploadId.id);

        setDeclarationUploadResult(uploadResult);
      } catch (error) {
        const errorResult: UploadResult = {
          batchId: "-",
          errors: ["Error in file parsing. Try again."],
          fileName: fileSelected.name,
          state: "Error",
          oldestEntryDate: undefined,
          newestEntryDate: undefined,
        };
        setDeclarationUploadResult(errorResult);
      } finally {
        setIsLoading(false);
      }

      setIsLoading(false);
    }
  };

  const errorCount = declarationUploadResult?.errors.length ?? 0;
  const disableSubmit =
    errorCount > 0 || !uploadId || declarationUploadResult === undefined;

  const disableUploadDeclarationButton =
    declarationUploadResult !== undefined && errorCount === 0;

  return {
    isLoading,
    declarationUploadResult,
    disableUploadDeclarationButton,
    handleUploadClick,
    handleFileChange,
    hiddenFileInputRef,
    disableSubmit,
    setIsLoading,
    uploadId,
    errorCount,
    cancelUpload,
  };
};
