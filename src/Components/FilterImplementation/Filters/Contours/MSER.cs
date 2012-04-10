using System;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.Features2D;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Contours
{
	internal class MSER : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _output;
		
		private readonly Property _areaThresholdProperty;
		private readonly Property _deltaProperty;
		private readonly Property _edgeBlurSizeProperty;
		private readonly Property _maxAreaProperty;
		private readonly Property _maxEvolutionProperty;
		private readonly Property _maxVariationProperty;
		private readonly Property _minAreaProperty;
		private readonly Property _minDiversityProperty;
		private readonly Property _minMarginProperty;
		
		public MSER()
		{
			_deltaProperty = new Property("Delta", FilterPropertyType.Integer);
			_maxAreaProperty = new Property("MaxArea", FilterPropertyType.Integer);
			_minAreaProperty = new Property("MinArea", FilterPropertyType.Integer);
			_maxVariationProperty = new Property("MaxVariation", FilterPropertyType.Float);
			_minDiversityProperty = new Property("MinDiversity", FilterPropertyType.Float);
			_maxEvolutionProperty = new Property("MaxEvolution", FilterPropertyType.Integer);
			_areaThresholdProperty = new Property("AreaThreshold", FilterPropertyType.Float);
			_minMarginProperty = new Property("MinMargin", FilterPropertyType.Float);
			_edgeBlurSizeProperty = new Property("EdgeBlurSize", FilterPropertyType.Integer);
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
			               	{
			               		Delta = (int) _deltaProperty.Value,
			               		MaxArea = (int) _maxAreaProperty.Value,
			               		MinArea = (int) _minAreaProperty.Value,
			               		MaxVariation = (float) _maxVariationProperty.Value,
			               		MinDiversity = (float) _minDiversityProperty.Value,
			               		MaxEvolution = (int) _maxEvolutionProperty.Value,
			               		AreaThreshold = (double) _areaThresholdProperty.Value,
			               		MinMargin = (double) _minMarginProperty.Value,
			               		EdgeBlurSize = (int) _edgeBlurSizeProperty.Value
			               	};

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}