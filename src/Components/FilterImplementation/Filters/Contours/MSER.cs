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
			_deltaProperty = new IntegerProperty("Delta", 1, 100, 1) { Value = mserDetector.Delta };
			_maxAreaProperty = new IntegerProperty("MaxArea", 10, 288000, 250) { Value = mserDetector.MaxArea };
			_minAreaProperty = new IntegerProperty("MinArea", 1, 16000, 100) { Value = mserDetector.MinArea };
			_maxVariationProperty = new FloatProperty("MaxVariation", 0, 1, (decimal)0.01f, 2) { Value = mserDetector.MaxVariation };
			_minDiversityProperty = new FloatProperty("MinDiversity", 0, 1, (decimal)0.01f, 2) { Value = mserDetector.MinDiversity };
			_maxEvolutionProperty = new IntegerProperty("MaxEvolution", 0, 1000, 1) { Value = mserDetector.MaxEvolution };
			_areaThresholdProperty = new FloatProperty("AreaThreshold", 0, 5, (decimal)0.01f, 2) { Value = (float)mserDetector.AreaThreshold };
			_minMarginProperty = new FloatProperty("MinMargin", 0, 1, (decimal)0.001f, 3) { Value = (float)mserDetector.MinMargin };
			_edgeBlurSizeProperty = new IntegerProperty("EdgeBlurSize", 0, 100, 1) { Value = mserDetector.EdgeBlurSize };
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
			detector.Dispose();
			image.Dispose();

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}