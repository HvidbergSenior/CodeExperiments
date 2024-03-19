export interface VolumeAndGhgInfo {
  volume: number;
  ghgReduction: number;
  volumeMultiplier: number;
}

export const calculateGhgWeightedAverage = (
  items: VolumeAndGhgInfo[],
): number => {
  var volumeBaselineSum = 0;
  var ghgBaselineSum = 0;
  for (let item of items) {
    const volumeBaseline = (item.volume * 34 * item.volumeMultiplier) / 1000000;
    // ghgReduction is transformed from fraction to percent by multiplying with 100
    const ghgBaseline = volumeBaseline * item.ghgReduction * 100;
    volumeBaselineSum += volumeBaseline;
    ghgBaselineSum += ghgBaseline;
  }
  const ghgWeighted =
    volumeBaselineSum === 0 ? 0 : ghgBaselineSum / volumeBaselineSum;
  // Return weighted average percent as fraction
  return ghgWeighted / 100;
};
