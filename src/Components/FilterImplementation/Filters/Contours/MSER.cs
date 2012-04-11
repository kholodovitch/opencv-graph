using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Features2D;
using FilterImplementation.Base;
using FilterImplementation.FilterProperties;

namespace FilterImplementation.Filters.Contours
{
	internal class MSER : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;

		private readonly FloatProperty _areaThresholdProperty;
		private readonly IntegerProperty _deltaProperty;
		private readonly IntegerProperty _edgeBlurSizeProperty;
		private readonly IntegerProperty _maxAreaProperty;
		private readonly IntegerProperty _maxEvolutionProperty;
		private readonly FloatProperty _maxVariationProperty;
		private readonly IntegerProperty _minAreaProperty;
		private readonly FloatProperty _minDiversityProperty;
		private readonly FloatProperty _minMarginProperty;
		
		public MSER()
		{
			var mserDetector = new MSERDetector();
			_deltaProperty = new IntegerProperty("Delta") {Value = mserDetector.Delta};
			_maxAreaProperty = new IntegerProperty("MaxArea") { Value = mserDetector.MaxArea };
			_minAreaProperty = new IntegerProperty("MinArea") { Value = mserDetector.MinArea };
			_maxVariationProperty = new FloatProperty("MaxVariation") { Value = mserDetector.MaxVariation };
			_minDiversityProperty = new FloatProperty("MinDiversity") { Value = mserDetector.MinDiversity };
			_maxEvolutionProperty = new IntegerProperty("MaxEvolution") { Value = mserDetector.MaxEvolution };
			_areaThresholdProperty = new FloatProperty("AreaThreshold") { Value = (float) mserDetector.AreaThreshold };
			_minMarginProperty = new FloatProperty("MinMargin") { Value = (float) mserDetector.MinMargin };
			_edgeBlurSizeProperty = new IntegerProperty("EdgeBlurSize") { Value = mserDetector.EdgeBlurSize };
			AddProperty(_deltaProperty);
			AddProperty(_maxAreaProperty);
			AddProperty(_minAreaProperty);
			AddProperty(_maxVariationProperty);
			AddProperty(_minDiversityProperty);
			AddProperty(_maxEvolutionProperty);
			AddProperty(_areaThresholdProperty);
			AddProperty(_minMarginProperty);
			AddProperty(_edgeBlurSizeProperty);

			_input = new InputPin("Image", PinMediaType.Image);
			_output = new OutputPin("Contours", PinMediaType.ContoursArray);
			AddPin(_input);
			AddPin(_output);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("DB3068F2-A3F6-4309-BCFC-0034E8EF1D60"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var image = (IImage) _input.GetData();
			var detector = new MSERDetector
			               	(
			               		_deltaProperty.Value,
			               		_maxAreaProperty.Value,
			               		_minAreaProperty.Value,
			               		_maxVariationProperty.Value,
			               		_minDiversityProperty.Value,
			               		_maxEvolutionProperty.Value,
			               		_areaThresholdProperty.Value,
			               		_minMarginProperty.Value,
			               		_edgeBlurSizeProperty.Value
			               	);
			using (var storage = new MemStorage())
				detector.ExtractContours(image, null, storage);

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}