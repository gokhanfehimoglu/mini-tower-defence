using System.Linq;
using UnityEngine;

namespace MiniTowerDefence
{
    public class RoadManager : Singleton<RoadManager>
    {
        [SerializeField]
        private RoadChain roadChain;

        private float _percentage;
        private RoadSegment[] _segments;
        private float[] _segmentLengths;
        private float _totalRoadLength;
        private OrientedCubicBezier3D[] _segmentCurves;

        private void Start()
        {
            _segments = roadChain.GetRoadSegments();
            _segmentCurves = new OrientedCubicBezier3D[_segments.Length];
            var i = 0;
            _segmentLengths = _segments.Select(x =>
            {
                var curve = x.GetBezierRepresentation(Space.World);
                _segmentCurves[i] = curve;
                i++;
                return curve.GetArcLength();
            }).ToArray();
            _totalRoadLength = _segmentLengths.Sum();
        }

        public OrientedPoint GetRoadPoint(float percentage)
        {
            var startDist = 0f;
            var startPercentage = 0f;
            for (var i = 0; i < _segments.Length; i++)
            {
                var endDist = startDist + _segmentLengths[i];
                var endPercentage = endDist / _totalRoadLength;

                if (percentage <= endPercentage)
                {
                    var localPercentage = (percentage - startPercentage) / (endPercentage - startPercentage);
                    return _segmentCurves[i].GetOrientedPoint(localPercentage);
                }

                startDist = endDist;
                startPercentage = endPercentage;
            }

            return default;
        }
    }
}