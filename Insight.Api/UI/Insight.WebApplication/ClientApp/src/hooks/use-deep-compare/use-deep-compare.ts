import { dequal as deepEqual } from "dequal";
import React from "react";

type UseEffectParams = Parameters<typeof React.useEffect>;
type UseEffectCallback = UseEffectParams[0];
type UseDependencyList = UseEffectParams[1];

type UseEffectReturn = ReturnType<typeof React.useEffect>;

const useDeepCompareMemo = <T>(value: T) => {
  const ref = React.useRef<T>(value);
  const signalRef = React.useRef<number>(0);

  if (!deepEqual(value, ref.current)) {
    ref.current = value;
    signalRef.current += 1;
  }

  // eslint-disable-next-line react-hooks/exhaustive-deps
  return React.useMemo(() => ref.current, [signalRef.current]);
};

/**
 * The useDeepCompare hook works like a normal React.useEffect hook.
 * But it can also compare neeply nested values like objects inside the depencency array.
 */
export const useDeepCompare = (
  callback: UseEffectCallback,
  dependencies: UseDependencyList,
): UseEffectReturn => {
  // eslint-disable-next-line react-hooks/exhaustive-deps
  return React.useEffect(callback, useDeepCompareMemo(dependencies));
};

export default useDeepCompare;
