export const deepCompareArrays = (arr1: any[], arr2: any[]): boolean => {
  // Check if both arguments are arrays
  if (!Array.isArray(arr1) || !Array.isArray(arr2)) {
    return false;
  }

  // Check if arrays have same length
  if (arr1.length !== arr2.length) {
    return false;
  }

  // Recursively compare each element
  for (let i = 0; i < arr1.length; i++) {
    // If elements are arrays, recursively compare them
    if (Array.isArray(arr1[i]) && Array.isArray(arr2[i])) {
      if (!deepCompareArrays(arr1[i], arr2[i])) {
        return false;
      }
    } else {
      // Otherwise, compare elements directly
      if (arr1[i] !== arr2[i]) {
        return false;
      }
    }
  }

  return true;
};
