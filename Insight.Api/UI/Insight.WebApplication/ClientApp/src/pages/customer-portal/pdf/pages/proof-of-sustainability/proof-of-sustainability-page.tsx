import CheckBoxOutlineBlankIcon from "@mui/icons-material/CheckBoxOutlineBlank";
import CheckBoxOutlinedIcon from "@mui/icons-material/CheckBoxOutlined";
import { Box, Divider, Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import {
  Address,
  Declarationinfo,
  PdfReportPosResponse,
  Recipient,
  Renewablefuelsupplier,
  Scopeofcertificationandghgemission,
  Scopeofcertificationofrawmaterial,
} from "../../../../../api/api";
import { theme } from "../../../../../theme/theme";
import { proofOfSustainabilityPageTranslations } from "../../../../../translations/pdf/proof-of-sustainability-page-translations";
import {
  formatDate,
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";
import { PdfContainer } from "../../pdf-container";
import "./../styles.css"; // adjust the path based on your project structure
import {
  GreenhouseGasEmissionsSavings,
  LifeCycleGreenhouseGasEmissions,
  RawMaterialSustainability,
  RenewableFuel,
} from "./mock-data";

export const SELECTOR_FOR_PDF_DOWNLOAD = "SelectorForPDFDownload";
const typoMain = "pdfSectionBody";
const typoMainBold = "pdfSectionBodyBold";
const typoMainLarge = "pdfSectionBodyLarge";
const typoMainSmall = "pdfSectionBodySmall";
const typoMainSmaller = "pdfSectionBodySmaller";
const typoMainSmallest = "pdfSectionBodySmallest";
const typoSectionHeader = "pdfSectionHeaderSmall";

const spaceBetweenSections = "0.6cm";
const labelColumnWidth = "3.4cm";
const sectionInternalGapHeight = "0.4cm";
const checkBoxColumnWidth = "0.9cm";
const checkBoxIconSize = "16px";
interface Props {
  pageNumber: number;
  proofOfSustainability: PdfReportPosResponse;
}
export const ProofOfSustainabilityPage = ({
  pageNumber,
  proofOfSustainability,
}: Props) => {
  return (
    <PdfContainer
      title={proofOfSustainabilityPageTranslations.pageTitle}
      pageNumber={pageNumber}
    >
      <Stack mt="0.7cm" display="flex">
        <DeclarationInfoView
          declarationInfo={proofOfSustainability.declarationinfo}
        />
        <Box height="0.7cm" />
        <Box
          display="grid"
          gap={spaceBetweenSections}
          gridTemplateColumns="1fr 1fr"
        >
          <RecipientView data={proofOfSustainability.recipient} />
          <RenewableFuelSupplierView
            data={proofOfSustainability.renewablefuelsupplier}
          />
        </Box>
        <Box height="0.7cm" />
        <Box
          display="grid"
          gap={spaceBetweenSections}
          gridTemplateColumns="1fr 1fr"
        >
          <RenewableFuelView data={proofOfSustainability.renewablefuel} />
          <ScopeOfCertificationAndGHGEmissionView
            data={proofOfSustainability.scopeofcertificationandghgemission}
          />
        </Box>
        <Box height="0.4cm" />
        <Box
          display="grid"
          gap={spaceBetweenSections}
          gridTemplateColumns="1fr 1fr"
        >
          <RawMaterialSustainabilityView
            data={proofOfSustainability.rawmaterialsustainability}
          />
          <ScopeOfCertificationOfRawMaterialView
            data={proofOfSustainability.scopeofcertificationofrawmaterial}
          />
        </Box>
        <Box height="0.7cm" />
        <Box
          display="grid"
          gap={spaceBetweenSections}
          gridTemplateColumns="1fr 1fr"
        >
          <LifeCycleGreenhouseGasEmissionsView
            data={proofOfSustainability.lifecyclegreenhousegasemissions}
          />
          <GreenhouseGasEmissionsSavingsView
            data={proofOfSustainability.greenhousegasemissionssavings}
          />
        </Box>
        <Box height="0.7cm" />
        <Box position="absolute" bottom="2.5cm" pr="2cm">
          <FooterView />
        </Box>
      </Stack>
    </PdfContainer>
  );
};

const DeclarationInfoView = ({
  declarationInfo,
}: {
  declarationInfo: Declarationinfo;
}) => {
  // TODO: BKN - date formatting
  return (
    <Box
      display="grid"
      gridTemplateColumns="3.5cm auto"
      bgcolor="#f2f2f2"
      pt="0.4cm"
      pb="0.4cm"
      pl="0.2cm"
      pr="0.2cm"
      className={SELECTOR_FOR_PDF_DOWNLOAD} // selector that must be visible before puppeteer downloads the pdf
    >
      <Typography variant={typoMainBold}>
        {proofOfSustainabilityPageTranslations.declarationID}
      </Typography>
      <Typography variant={typoMainBold}>{declarationInfo.id}</Typography>
      <Typography variant={typoMain}>
        {proofOfSustainabilityPageTranslations.dateOfIssuance}
      </Typography>
      <Typography variant={typoMain}>
        {formatDate(new Date(declarationInfo.dateOfIssuance))}
      </Typography>
    </Box>
  );
};

const SectionHeaderView = ({ title }: { title: string }) => {
  return (
    <Stack>
      <Typography variant={typoSectionHeader}>{title}</Typography>
      <Divider color={theme.palette.primary.light}></Divider>
    </Stack>
  );
};

const AddressView = ({ address }: { address: Address }) => {
  return (
    <Stack>
      <Typography variant={typoMain}>{address.name}</Typography>
      <Typography variant={typoMain}>
        {address.street} {address.streetNumber}
      </Typography>
      <Typography variant={typoMain}>
        {address.zipCode} {address.city}
      </Typography>
      <Typography variant={typoMain}>{address.country}</Typography>
    </Stack>
  );
};

const RecipientView = ({ data }: { data: Recipient }) => {
  // TODO: BKN - better date formatting
  return (
    <Stack gap={sectionInternalGapHeight}>
      <SectionHeaderView
        title={proofOfSustainabilityPageTranslations.recipient.title}
      />
      <AddressView address={data.address} />
      <Box display="grid" gridTemplateColumns={`${labelColumnWidth} auto`}>
        <Typography variant={typoMain}>
          {proofOfSustainabilityPageTranslations.recipient.period}
        </Typography>
        <Typography variant={typoMain}>
          {formatDate(new Date(data.periodFrom))}
          {" to "}
          {formatDate(new Date(data.periodTo))}
        </Typography>
      </Box>
    </Stack>
  );
};

const RenewableFuelSupplierView = ({
  data,
}: {
  data: Renewablefuelsupplier;
}) => {
  return (
    <Stack gap={sectionInternalGapHeight}>
      <SectionHeaderView
        title={
          proofOfSustainabilityPageTranslations.renewableFuelSupplier.title
        }
      />
      <AddressView address={data.address} />
      <Box display="grid" gridTemplateColumns={`${labelColumnWidth} auto`}>
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.renewableFuelSupplier
              .certificateSystem
          }
        </Typography>
        <Typography variant={typoMain}>{data.certificateSystem}</Typography>
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.renewableFuelSupplier
              .certificateNumber
          }
        </Typography>
        <Typography variant={typoMain}>{data.certificateNumber}</Typography>
      </Box>
    </Stack>
  );
};

const RenewableFuelView = ({ data }: { data: RenewableFuel }) => {
  // TODO: BKN - format numbers with delimiters
  return (
    <Stack>
      <SectionHeaderView
        title={proofOfSustainabilityPageTranslations.renewableFuel.title}
      />
      <Box height={sectionInternalGapHeight} />
      <Box
        display="grid"
        gridTemplateColumns={`${labelColumnWidth} auto`}
        rowGap="0.2cm"
      >
        <Typography variant={typoMain}>
          {proofOfSustainabilityPageTranslations.renewableFuel.volume}
        </Typography>
        <Typography variant={typoMainLarge}>
          {formatNumber(data.volume) +
            " " +
            proofOfSustainabilityPageTranslations.renewableFuel.volumeLUnit}
        </Typography>
        <Typography variant={typoMain}>
          {proofOfSustainabilityPageTranslations.renewableFuel.product}
        </Typography>
        <Typography variant={typoMainLarge}>{data.product}</Typography>
        <Typography variant={typoMain}>
          {proofOfSustainabilityPageTranslations.renewableFuel.energyContent}
        </Typography>
        <Typography variant={typoMainBold}>
          {formatNumber(data.energyContent)}{" "}
          {
            proofOfSustainabilityPageTranslations.renewableFuel
              .energyContentUnit
          }
        </Typography>
      </Box>
    </Stack>
  );
};

const CheckBoxRowView = ({ text, value }: { text: string; value: boolean }) => {
  return (
    <Box
      display="grid"
      gridTemplateColumns={`auto ${checkBoxColumnWidth} ${checkBoxColumnWidth}`}
    >
      <Typography variant={typoMainSmall} alignSelf="center">
        {text}
      </Typography>
      <Box justifySelf="center">
        {value ? (
          <CheckBoxOutlinedIcon sx={{ fontSize: checkBoxIconSize }} />
        ) : (
          <CheckBoxOutlineBlankIcon sx={{ fontSize: checkBoxIconSize }} />
        )}
      </Box>
      <Box justifySelf="center">
        {!value ? (
          <CheckBoxOutlinedIcon sx={{ fontSize: checkBoxIconSize }} />
        ) : (
          <CheckBoxOutlineBlankIcon sx={{ fontSize: checkBoxIconSize }} />
        )}
      </Box>
    </Box>
  );
};

const ScopeOfCertificationAndGHGEmissionView = ({
  data,
}: {
  data: Scopeofcertificationandghgemission;
}) => {
  return (
    <Stack>
      <SectionHeaderView
        title={
          proofOfSustainabilityPageTranslations
            .scopeOfCertificationAndGHGEmission.title
        }
      />
      <Stack rowGap="0.05cm">
        <Box
          display="grid"
          gridTemplateColumns={`auto ${checkBoxColumnWidth} ${checkBoxColumnWidth}`}
        >
          <Typography variant={typoMainBold}></Typography>

          <Typography variant={typoMainBold} justifySelf="center">
            {proofOfSustainabilityPageTranslations.optionYes}
          </Typography>
          <Typography variant={typoMainBold} justifySelf="center">
            {proofOfSustainabilityPageTranslations.optionNo}
          </Typography>
        </Box>

        <CheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationAndGHGEmission.euRedCompliantMaterial
          }
          value={data.euRedCompliantMaterial}
        />
        <CheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationAndGHGEmission.isccCompliantMaterial
          }
          value={data.isccCompliantMaterial}
        />
        <Box display="grid" gridTemplateColumns={`4fr 1fr`}>
          <Typography variant={typoMainSmall}>
            {
              proofOfSustainabilityPageTranslations
                .scopeOfCertificationAndGHGEmission.chainOfCustodyOption
            }
          </Typography>
          <Typography variant={typoMainBold} alignSelf="center">
            [{data.chainOfCustodyOption}]
          </Typography>
        </Box>
        <CheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationAndGHGEmission
              .totalDefaultValueAccordingToRed2Applied
          }
          value={data.totalDefaultValueAccordingToRed2Applied}
        />
      </Stack>
    </Stack>
  );
};

const RawMaterialSustainabilityView = ({
  data,
}: {
  data: RawMaterialSustainability;
}) => {
  return (
    <Stack>
      <SectionHeaderView
        title={
          proofOfSustainabilityPageTranslations.rawMaterialSustainability.title
        }
      />
      <Box height={sectionInternalGapHeight} />
      <Box
        display="grid"
        gridTemplateColumns={`${labelColumnWidth} auto`}
        rowGap="0.2cm"
      >
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.rawMaterialSustainability
              .rawMaterial
          }
        </Typography>
        <Typography variant={typoMain}>{data.rawMaterial}</Typography>
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.rawMaterialSustainability
              .countryOfOrigin
          }
        </Typography>
        <Typography variant={typoMain}>{data.countryOfOrigin}</Typography>
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.rawMaterialSustainability
              .productionCountry
          }
        </Typography>
        <Typography variant={typoMain}>{data.productionCountry}</Typography>
        <Typography variant={typoMain}>
          {
            proofOfSustainabilityPageTranslations.rawMaterialSustainability
              .dateOfInstallation
          }
        </Typography>
        <Typography variant={typoMain}>
          {formatDate(new Date(data.dateOfInstallation))}
        </Typography>
      </Box>
    </Stack>
  );
};

const SmallCheckBoxRowView = ({
  text,
  value,
}: {
  text: string;
  value: boolean;
}) => {
  return (
    <Box
      display="grid"
      gridTemplateColumns={`auto ${checkBoxColumnWidth} ${checkBoxColumnWidth}`}
    >
      <Typography variant={typoMainSmaller} alignSelf="center">
        {text}
      </Typography>
      <Box justifySelf="center">
        {value ? (
          <CheckBoxOutlinedIcon sx={{ fontSize: checkBoxIconSize }} />
        ) : (
          <CheckBoxOutlineBlankIcon sx={{ fontSize: checkBoxIconSize }} />
        )}
      </Box>
      <Box justifySelf="center">
        {!value ? (
          <CheckBoxOutlinedIcon sx={{ fontSize: checkBoxIconSize }} />
        ) : (
          <CheckBoxOutlineBlankIcon sx={{ fontSize: checkBoxIconSize }} />
        )}
      </Box>
    </Box>
  );
};

const ScopeOfCertificationOfRawMaterialView = ({
  data,
}: {
  data: Scopeofcertificationofrawmaterial;
}) => {
  return (
    <Stack>
      <SectionHeaderView
        title={
          proofOfSustainabilityPageTranslations
            .scopeOfCertificationOfRawMaterial.title
        }
      />

      <Stack rowGap="0.05cm">
        <Box
          display="grid"
          gridTemplateColumns={`auto ${checkBoxColumnWidth} ${checkBoxColumnWidth}`}
        >
          <Typography variant={typoMainBold}></Typography>

          <Typography variant={typoMainBold} justifySelf="center">
            {proofOfSustainabilityPageTranslations.optionYes}
          </Typography>
          <Typography variant={typoMainBold} justifySelf="center">
            {proofOfSustainabilityPageTranslations.optionNo}
          </Typography>
        </Box>

        <SmallCheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationOfRawMaterial.option1
          }
          value={data.option1}
        />
        <SmallCheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationOfRawMaterial.option2
          }
          value={data.option2}
        />
        <SmallCheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationOfRawMaterial.option3
          }
          value={data.option3}
        />
        <SmallCheckBoxRowView
          text={
            proofOfSustainabilityPageTranslations
              .scopeOfCertificationOfRawMaterial.option4
          }
          value={data.option4}
        />
        <Box display="grid" gridTemplateColumns={`4fr 1fr`}>
          <Typography variant={typoMainSmall}>
            {
              proofOfSustainabilityPageTranslations
                .scopeOfCertificationOfRawMaterial.option5
            }
          </Typography>
          <Typography variant={typoMainBold}>[{data.option5}]</Typography>
        </Box>
      </Stack>
    </Stack>
  );
};

const lifecycleSectionColumn1Widht = "1.3cm";

const LifecycleGHGRowView = ({
  prefix,
  text,
  value,
}: {
  prefix: string;
  text: string;
  value: number;
}) => {
  return (
    <Box
      display="grid"
      gridTemplateColumns={`${lifecycleSectionColumn1Widht} auto 1.3cm`}
    >
      <Typography variant={typoMain}>{prefix}</Typography>
      <Typography variant={typoMain}>{text}</Typography>
      {value > 0 ? (
        <Typography variant={typoMain} justifySelf="end">
          {value}
        </Typography>
      ) : (
        <div></div>
      )}
    </Box>
  );
};

const LifeCycleGreenhouseGasEmissionsView = ({
  data,
}: {
  data: LifeCycleGreenhouseGasEmissions;
}) => {
  // TODO: BKN - format numbers
  return (
    <Stack>
      <SectionHeaderView
        title={proofOfSustainabilityPageTranslations.lifeCycle.title}
      />
      <Box height={sectionInternalGapHeight} />

      <Stack>
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.eec}
          text={proofOfSustainabilityPageTranslations.lifeCycle.eecText}
          value={data.extractionOrCultivation}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.el}
          text={proofOfSustainabilityPageTranslations.lifeCycle.elText}
          value={data.landUse}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.ep}
          text={proofOfSustainabilityPageTranslations.lifeCycle.epText}
          value={data.processing}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.etd}
          text={proofOfSustainabilityPageTranslations.lifeCycle.etdText}
          value={data.transportAndDistribution}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.eu}
          text={proofOfSustainabilityPageTranslations.lifeCycle.euText}
          value={data.fuelInUse}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.esca}
          text={proofOfSustainabilityPageTranslations.lifeCycle.escaText}
          value={data.soilCarbonAccumulation}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.eccs}
          text={proofOfSustainabilityPageTranslations.lifeCycle.eccsText}
          value={data.carbonCaptureAndGeologicalStorage}
        />
        <LifecycleGHGRowView
          prefix={proofOfSustainabilityPageTranslations.lifeCycle.eccr}
          text={proofOfSustainabilityPageTranslations.lifeCycle.eccrText}
          value={data.carbonCaptureAndReplacement}
        />
        <Box
          display="grid"
          gridTemplateColumns={`${lifecycleSectionColumn1Widht} auto 2.3cm`}
          mt="0.2cm"
        >
          <Typography variant={typoMainBold}>
            {proofOfSustainabilityPageTranslations.lifeCycle.e}
          </Typography>
          <Typography variant={typoMainBold}>
            {proofOfSustainabilityPageTranslations.lifeCycle.eText}
          </Typography>
          <Stack justifySelf="end" alignItems="end">
            <Typography variant={typoMainBold}>
              {data.totalGHGEmissionFromSupplyAndUseOfFuel}
            </Typography>
            <Typography variant={typoMainBold}>
              {proofOfSustainabilityPageTranslations.lifeCycle.eUnit}
            </Typography>
          </Stack>
        </Box>
      </Stack>
    </Stack>
  );
};

const GreenhouseGasEmissionsSavingsView = ({
  data,
}: {
  data: GreenhouseGasEmissionsSavings;
}) => {
  const optionsSpeedometer: ApexOptions = {
    chart: {
      height: 280,
      type: "radialBar",
    },
    series: [data.ghgPercent],
    colors: [theme.palette.primary.main],
    plotOptions: {
      radialBar: {
        startAngle: -90,
        endAngle: 90,
        track: {
          background: theme.palette.common.black,
          startAngle: -90,
          endAngle: 90,
        },
        dataLabels: {
          name: {
            show: false,
          },
          value: {
            fontSize: "30px",
            show: false,
          },
        },
      },
    },
    fill: {
      type: "gradient",
      gradient: {
        shade: "dark",
        type: "horizontal",
        gradientToColors: [theme.palette.primary.main],
        stops: [0, 100],
      },
    },
    stroke: {
      lineCap: "butt",
    },
  };

  return (
    <Box
      sx={{
        border: 1,
        borderColor: theme.palette.primary.main,
        height: "200px",
        mt: "-0.15cm",
      }}
    >
      <Stack position="relative">
        <Box
          bgcolor={theme.palette.primary.main}
          pt="0.30cm"
          pb="0.30cm"
          pl="0.1cm"
          pr="0.1cm"
          display="flex"
          flexDirection="row"
          justifyContent="center"
        >
          <Typography
            variant={typoSectionHeader}
            sx={{ color: theme.palette.common.white, textAlign: "center" }}
          >
            {
              proofOfSustainabilityPageTranslations
                .greenhouseGasEmissionsSavings.title
            }
          </Typography>
        </Box>

        <Stack alignItems="center" sx={{ position: "relative" }}>
          <Box mt="-0.5cm">
            <Box
              position="absolute"
              sx={{ top: "0.9cm", left: "2.5cm" }}
              width="3.3cm"
              height="3.3cm"
            >
              <Box
                position="relative"
                display="flex"
                width="100%"
                height="100%"
                alignItems="center"
                justifyContent="center"
                flexDirection="column"
              >
                <Typography variant="h3" fontSize="32px">
                  {formatPercentage(data.ghgPercent, 0)}
                </Typography>
              </Box>
            </Box>
            <ReactApexChart
              options={optionsSpeedometer}
              series={optionsSpeedometer.series}
              type="radialBar"
              height={280}
            />
          </Box>
        </Stack>
        <Box
          mt="-1.7cm"
          display="flex"
          flexDirection="column"
          alignItems="center"
        >
          <Typography variant={typoMainSmaller}>
            {
              proofOfSustainabilityPageTranslations
                .greenhouseGasEmissionsSavings.info1
            }
          </Typography>
          <Box height="0.05cm" />
          <Typography variant={typoMainSmaller} sx={{ fontStyle: "italic" }}>
            {
              proofOfSustainabilityPageTranslations
                .greenhouseGasEmissionsSavings.info2
            }
          </Typography>
        </Box>
      </Stack>
    </Box>
  );
};

const FooterView = () => {
  return (
    <Typography variant={typoMainSmallest} style={{ whiteSpace: "pre-line" }}>
      {proofOfSustainabilityPageTranslations.footer.info}
    </Typography>
  );
};
