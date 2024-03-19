import { useInView } from "react-intersection-observer";

export const useInfiniteScrolling = (
  callback: () => void,
  hasMore: boolean,
  isLoading: boolean,
  options: IntersectionObserverInit = {
    // Number from 0 to 1 indicating percentage of element that should be visible
    threshold: 0,
    // Here you can pass a ref so visibility is determined within its region. Null makes visibility bound to viewport
    root: null,
    // Margin around root element
    rootMargin: "0px",
  },
) => {
  const { ref } = useInView({
    threshold: options.threshold,
    root: options.root ? (options.root as Element) : options.root,
    rootMargin: options.rootMargin,
    onChange: (inView) => {
      if (inView && hasMore && !isLoading) {
        callback();
      }
    },
  });
  return ref;
};
